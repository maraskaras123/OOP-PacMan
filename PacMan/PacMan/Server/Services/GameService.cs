using System.Drawing;
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;
using PacMan.Server.Hubs;
using PacMan.Shared;
using PacMan.Shared.Enums;
using PacMan.Shared.Factories;
using PacMan.Shared.Factories.PacMan.Shared.Factories;
using PacMan.Shared.Models;

namespace PacMan.Server.Services
{
    public interface IGameService
    {
        void Reset(bool? clearPlayers);
        void Start(TileGridBuilderOptions gridOptions);
        Task Init();
        void Finish();
    }

    public class GameService : IGameService
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;
        private EnemyFactory _redGhostFactory;
        private EnemyFactory _blueGhostFactory;
        private TileFactory emptyTileFactory;

        public GameService(IHubContext<GameHub, IGameHubClient> hubContext)
        {
            _redGhostFactory = new RedGhostFactory();
            _blueGhostFactory = new BlueGhostFactory();
            _hubContext = hubContext;
        }

        public void Reset(bool? clearPlayers)
        {
            Storage.GameState = EnumGameState.Initializing;
            if (clearPlayers ?? false)
            {
                Storage.ConnectionIds = new();
            }
        }

        public void Start(TileGridBuilderOptions gridOptions)
        {
            CreateEnemies();
            Storage.GameState = EnumGameState.Starting;
            TileGrid grid = new TileGridBuilder()
                .WithWidth(gridOptions.Width)
                .WithHeight(gridOptions.Height)
                .WithRandomTiles(gridOptions.RandomTileCount)
                .Build();
            Storage.Grid = grid;
        }

        private void CreateEnemies()
        {
            var redGhost = _redGhostFactory.CreateEnemy();
            var blueGhost = _blueGhostFactory.CreateEnemy();
            redGhost.Position = new Point(10, 10);
            blueGhost.Position = new Point(9, 9);
            Storage.Enemies.Add(redGhost);
            Storage.Enemies.Add(blueGhost);
        }

        public async Task Init()
        {
            emptyTileFactory = new EmptyTileFactory();
            Storage.GameState = EnumGameState.Running;
            Storage.Ticks = 0;

            foreach (var connectionId in Storage.ConnectionIds)
            {
                var stateModel = new GameStateModel();
                Storage.State.Add(connectionId, stateModel);


                await _hubContext.Clients.All.ReceiveGrid(Storage.Grid.ConvertForSending());
            }
            while (Storage.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(500), Tick());
                await _hubContext.Clients.All.ReceiveGrid(Storage.Grid.ConvertForSending());
                await _hubContext.Clients.All.Tick(new(Storage.GameState,
                    Storage.State.Select((x, index) => $"{index},{x.Value.Coordinates.X},{x.Value.Coordinates.Y}")
                        .ToList(),
                    Storage.State.Select(x => x.Value.Points).ToList()));
            }

        }

        public void Finish()
        {
            Storage.GameState = EnumGameState.Finished;
        }

        // Game Logic
        private async Task Tick()
        {
            await _hubContext.Clients.All.ReceiveGrid(Storage.Grid.ConvertForSending());
            foreach (var enemy in Storage.Enemies)
            {
                enemy.Move(Storage.State);
            }
            foreach (var state in Storage.State)
            {
                int currentX = state.Value.Coordinates.X;
                int currentY = state.Value.Coordinates.Y;
                Tile desiredTile = new EmptyTile();
                switch (state.Value.Direction)
                {
                    case EnumDirection.Up:
                        desiredTile = Storage.Grid.GetTile(currentX, currentY - 1);
                        if (state.Value.Coordinates.Y > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            state.Value.Coordinates = new(currentX, currentY - 1);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = emptyTileFactory.CreateTile() as EmptyTile;
                                Storage.Grid.ChangeTile(desiredTile, currentX, currentY - 1);
                                state.Value.Points += 1;
                            }
                        }
                        break;
                    case EnumDirection.Right:
                        desiredTile = Storage.Grid.GetTile(currentX + 1, currentY);
                        if (state.Value.Coordinates.X < Storage.Grid.Width - 1 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(1, 0);
                            state.Value.Coordinates = new(currentX + 1, currentY);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = emptyTileFactory.CreateTile() as EmptyTile;
                                Storage.Grid.ChangeTile(desiredTile, currentX + 1, currentY);
                                state.Value.Points += 1;
                            }
                        }
                        break;
                    case EnumDirection.Down:
                        desiredTile = Storage.Grid.GetTile(currentX, currentY + 1);
                        if (state.Value.Coordinates.Y < Storage.Grid.Height - 1 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(0, 1);
                            state.Value.Coordinates = new(currentX, currentY + 1);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = emptyTileFactory.CreateTile() as EmptyTile;
                                Storage.Grid.ChangeTile(desiredTile, currentX, currentY + 1);
                                state.Value.Points += 1;
                            }
                        }
                        break;
                    case EnumDirection.Left:
                        desiredTile = Storage.Grid.GetTile(currentX - 1, currentY);
                        if (state.Value.Coordinates.X > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(-1, 0);
                            state.Value.Coordinates = new(currentX - 1, currentY);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = emptyTileFactory.CreateTile() as EmptyTile;
                                Storage.Grid.ChangeTile(desiredTile, currentX - 1, currentY);
                                state.Value.Points += 1;
                            }
                        }
                        break;
                }
            }
            var enemyData = Storage.Enemies.Select(e => new EnemyModel
            {
                Position = e.Position,
                Character = e.Character
            }).ToList();
            await _hubContext.Clients.All.ReceiveEnemies(enemyData);
            Storage.Ticks += 1;
        }
    }
}