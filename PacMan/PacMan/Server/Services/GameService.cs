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
    }

    public class GameService : IGameService
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;
        private readonly EnemyFactory _redGhostFactory;
        private readonly EnemyFactory _blueGhostFactory;
        private readonly TileFactory _emptyTileFactory;
        private int _endPoints;

        public GameService(IHubContext<GameHub, IGameHubClient> hubContext)
        {
            _redGhostFactory = new RedGhostFactory();
            _blueGhostFactory = new BlueGhostFactory();
            _emptyTileFactory = new EmptyTileFactory();
            _hubContext = hubContext;
            _endPoints = 100;
        }

        public void Reset(GameStateModel session)
        {
            session.GameState = EnumGameState.Initializing;
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
                var stateModel = new PlayerStateModel()
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

            while (session.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(500), Tick(sessionId, session));
                await _hubContext.Clients.Group(sessionId).ReceiveGrid(session.Grid.ConvertForSending());
                await _hubContext.Clients.Group(sessionId).Tick(new(session.GameState,
                    session.State.Select((x, index) => $"{index},{x.Value.Coordinates.X},{x.Value.Coordinates.Y}")
                        .ToList(),
                    session.State.Select(x => x.Value.Points).ToList()));
            }
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
                var currentX = state.Value.Coordinates.X;
                var currentY = state.Value.Coordinates.Y;
                Tile desiredTile;
                switch (state.Value.Direction)
                {
                    case EnumDirection.Up:
                        desiredTile = session.Grid.GetTile(currentX, currentY - 1);
                        if (state.Value.Coordinates.Y > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            state.Value.Coordinates = new(currentX, currentY - 1);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                session.Grid.ChangeTile(_emptyTileFactory.ConvertToEmpty(), currentX, currentY - 1);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                    case EnumDirection.Right:
                        desiredTile = session.Grid.GetTile(currentX + 1, currentY);
                        if (state.Value.Coordinates.X < session.Grid.Width - 1 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(1, 0);
                            state.Value.Coordinates = new(currentX + 1, currentY);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                session.Grid.ChangeTile(_emptyTileFactory.ConvertToEmpty(), currentX + 1, currentY);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                    case EnumDirection.Down:
                        desiredTile = session.Grid.GetTile(currentX, currentY + 1);
                        if (state.Value.Coordinates.Y < session.Grid.Height - 1 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(0, 1);
                            state.Value.Coordinates = new(currentX, currentY + 1);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                session.Grid.ChangeTile(_emptyTileFactory.ConvertToEmpty(), currentX, currentY + 1);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                    case EnumDirection.Left:
                        desiredTile = session.Grid.GetTile(currentX - 1, currentY);
                        if (state.Value.Coordinates.X > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(-1, 0);
                            state.Value.Coordinates = new(currentX - 1, currentY);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                session.Grid.ChangeTile(_emptyTileFactory.ConvertToEmpty(), currentX - 1, currentY);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                }

                if (state.Value.Points >= _endPoints)
                {
                    session.GameState = EnumGameState.Finished;
                }
            }

            var enemyData = session.Enemies
                .Select(e => new EnemyModel { Position = e.Position, Character = e.Character })
                .ToList();
            await _hubContext.Clients.Group(sessionId).ReceiveEnemies(enemyData);
            session.Ticks += 1;
        }
    }
}