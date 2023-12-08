using Microsoft.AspNetCore.SignalR;
using PacMan.Server.Hubs;
using PacMan.Shared.Enums;
using PacMan.Shared.Factories;
using PacMan.Shared.Models;
using System.Drawing;

namespace PacMan.Server.Services
{
    public interface IGameService
    {
        void Reset(GameStateModel session);
        void Start(GameStateModel session, TileGridBuilderOptions gridOptions, int endPoints);
        Task Init(string sessionId, GameStateModel session);
        Task PlayerUpdate(string connectionId, int index, string name);
    }

    public class GameService : IGameService
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;
        private readonly EnemyFactory _redGhostFactory;
        private readonly EnemyFactory _blueGhostFactory;
        private readonly TileFactory _emptyTileFactory;
        private readonly TileFactory _megaPelletTileFactory;
        private int _endPoints;

        public GameService(IHubContext<GameHub, IGameHubClient> hubContext)
        {
            _redGhostFactory = new RedGhostFactory();
            _blueGhostFactory = new BlueGhostFactory();
            _emptyTileFactory = new EmptyTileFactory();
            _megaPelletTileFactory = new MegaPelletTileFactory();
            _hubContext = hubContext;
            _endPoints = 100;
        }

        public void Reset(GameStateModel session)
        {
            session.GameState = EnumGameState.Initializing;
            session.Enemies = new();
            session.Grid = new();
            session.Ticks = 0;
            session.State = new();
        }

        public void Start(GameStateModel session, TileGridBuilderOptions gridOptions, int points)
        {
            CreateEnemies(session);
            session.GameState = EnumGameState.Starting;
            var grid = new TileGridBuilder()
                .WithWidth(gridOptions.Width)
                .WithHeight(gridOptions.Height)
                .WithRandomTiles(gridOptions.RandomTileCount)
                .Build();
            session.Grid = grid;
            _endPoints = points;
        }

        private void CreateEnemies(GameStateModel session)
        {
            var redGhost = _redGhostFactory.CreateEnemy();
            var blueGhost = _blueGhostFactory.CreateEnemy();
            redGhost.Position = new(10, 10);
            blueGhost.Position = new(9, 9);
            session.Enemies.Add(redGhost);
            session.Enemies.Add(blueGhost);
        }

        public async Task Init(string sessionId, GameStateModel session)
        {
            session.GameState = EnumGameState.Running;
            session.Ticks = 0;
            var rnd = new Random();
            foreach (var connection in session.Connections)
            {
                var stateModel = new PlayerStateModel
                { Name = connection.Key, Direction = (EnumDirection)rnd.Next(0, 4) };
                var coordinates = new Point(rnd.Next(session.Grid.Width), rnd.Next(session.Grid.Height));
                while (session.Grid.Tiles[$"{coordinates.X}_{coordinates.Y}"].Type == EnumTileType.Wall)
                {
                    coordinates = new(rnd.Next(session.Grid.Width), rnd.Next(session.Grid.Height));
                }

                stateModel.Coordinates = coordinates;
                session.State.Add(connection.Key, stateModel);

                await _hubContext.Clients.Group(sessionId).ReceiveGrid(session.Grid.ConvertForSending());
            }

            await _hubContext.Clients.Group(sessionId).StateChange(session.GameState);

            while (session.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(500), Tick(sessionId, session));
                await _hubContext.Clients.Group(sessionId).ReceiveGrid(session.Grid.ConvertForSending());
                await _hubContext.Clients.Group(sessionId).Tick(new(session.GameState,
                    session.State.Select((x, index) =>
                            $"{index},{x.Value.Coordinates.X},{x.Value.Coordinates.Y},{(int)x.Value.Direction}")
                        .ToList(),
                    session.State.Select(x => x.Value.Points).ToList()));
            }
        }

        public async Task PlayerUpdate(string connectionId, int index, string name)
        {
            await _hubContext.Clients.Client(connectionId).PlayerUpdate(index, name);
        }

        private void TouchingEnemy(GameStateModel session, PlayerStateModel state)
        {
            foreach (var enemy in session.Enemies.Where(enemy => state.Coordinates == enemy.Position))
            {
                state.CollideWithGhost();
                enemy.Respawn(session);
            }
        }

        private bool IsOcupiedByPacman(GameStateModel session, int desiredX, int desiredY)
        {
            Point desiredPoint = new Point(desiredX, desiredY);
            foreach (var pacman in session.State)
            {
                if (pacman.Value.Coordinates == desiredPoint)
                {
                    return true;
                }
            }
            return false;
        }

        // Game Logic
        private async Task Tick(string sessionId, GameStateModel session)
        {
            await _hubContext.Clients.Group(sessionId).ReceiveGrid(session.Grid.ConvertForSending());
            foreach (var enemy in session.Enemies)
            {
                enemy.Move(session);
            }


            foreach (var state in session.State)
            {
                TouchingEnemy(session, state.Value);
                var currentX = state.Value.Coordinates.X;
                var currentY = state.Value.Coordinates.Y;
                var desiredX = currentX;
                var desiredY = currentY;

                switch (state.Value.Direction)
                {
                    case EnumDirection.Up:
                        desiredY--;
                        break;
                    case EnumDirection.Down:
                        desiredY++;
                        break;
                    case EnumDirection.Left:
                        desiredX--;
                        break;
                    case EnumDirection.Right:
                        desiredX++;
                        break;
                }
                switch (session.Grid.GetTile(desiredX, desiredY).Type)
                {
                    case EnumTileType.Wall:
                        break;
                    case EnumTileType.Pellet:
                        session.Grid.ChangeTile(_emptyTileFactory.ConvertToEmpty(), desiredX, desiredY);
                        state.Value.EatPellet();
                        goto case EnumTileType.Empty;
                    case EnumTileType.MegaPellet:
                        state.Value.SetState(new SuperPacmanState(state.Value));
                        session.Grid.ChangeTile(_emptyTileFactory.ConvertToEmpty(), desiredX, desiredY);
                        state.Value.SetToNormalTick(10);
                        goto case EnumTileType.Empty;
                    case EnumTileType.Empty:
                        state.Value.Coordinates = new(desiredX, desiredY);
                        break;
                }

                TouchingEnemy(session, state.Value);

                if (state.Value.Points >= _endPoints)
                {
                    session.GameState = EnumGameState.Finished;
                }
                state.Value.TickToNormalState();
            }

            var enemyData = session.Enemies
                .Select(e => new EnemyModel
                { Position = new() { X = e.Position.X, Y = e.Position.Y }, Character = e.Character })
                .ToList();
            await _hubContext.Clients.Group(sessionId).ReceiveEnemies(enemyData);
            if (session.Ticks % 10 == 0)
            {
                var rnd = new Random();
                bool placed = false;
                while (!placed)
                {
                    int x = rnd.Next(session.Grid.Width);
                    int y = rnd.Next(session.Grid.Height);
                    if (session.Grid.GetTile(x, y).Type != EnumTileType.Wall)
                    {
                        session.Grid.ChangeTile(_megaPelletTileFactory.CreateTile(), x, y);
                        placed = true;
                    }
                }
            }
            session.Ticks += 1;
        }
    }
}