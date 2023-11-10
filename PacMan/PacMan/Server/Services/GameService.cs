using Microsoft.AspNetCore.SignalR;
using PacMan.Server.Hubs;
using PacMan.Shared;
using PacMan.Shared.Enums;
using PacMan.Shared.Factories;
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
        private readonly EnemyFactory _redGhostFactory;
        private readonly EnemyFactory _blueGhostFactory;
        private readonly TileFactory _emptyTileFactory;

        public GameService(IHubContext<GameHub, IGameHubClient> hubContext)
        {
            _redGhostFactory = new RedGhostFactory();
            _blueGhostFactory = new BlueGhostFactory();
            _emptyTileFactory = new EmptyTileFactory();
            _hubContext = hubContext;
        }

        public void Reset(bool? clearPlayers)
        {
            var storage = Storage.GetInstance();
            storage.GameState = EnumGameState.Initializing;
            if (clearPlayers ?? false)
            {
                storage.ConnectionIds = new();
            }
        }

        public void Start(TileGridBuilderOptions gridOptions)
        {
            var storage = Storage.GetInstance();
            CreateEnemies();
            storage.GameState = EnumGameState.Starting;
            var grid = new TileGridBuilder()
                .WithWidth(gridOptions.Width)
                .WithHeight(gridOptions.Height)
                .WithRandomTiles(gridOptions.RandomTileCount)
                .Build();
            storage.Grid = grid;
        }


        private void CreateEnemies()
        {
            var storage = Storage.GetInstance();
            var redGhost = _redGhostFactory.CreateEnemy();
            var blueGhost = _blueGhostFactory.CreateEnemy();
            redGhost.Position = new(10, 10);
            blueGhost.Position = new(9, 9);
            storage.Enemies.Add(redGhost);
            storage.Enemies.Add(blueGhost);
        }

        public async Task Init()
        {
            var storage = Storage.GetInstance();
            storage.GameState = EnumGameState.Running;
            storage.Ticks = 0;
            foreach (var connectionId in storage.ConnectionIds)
            {
                var stateModel = new GameStateModel();
                storage.State.Add(connectionId, stateModel);


                await _hubContext.Clients.All.ReceiveGrid(storage.Grid.ConvertForSending());
            }

            while (storage.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(500), Tick());
                await _hubContext.Clients.All.ReceiveGrid(storage.Grid.ConvertForSending());
                await _hubContext.Clients.All.Tick(new(storage.GameState,
                    storage.State.Select((x, index) => $"{index},{x.Value.Coordinates.X},{x.Value.Coordinates.Y}")
                        .ToList(),
                    storage.State.Select(x => x.Value.Points).ToList()));
            }

        }

        public void Finish()
        {
            var storage = Storage.GetInstance();
            storage.GameState = EnumGameState.Finished;
        }

        // Game Logic
        private async Task Tick()
        {
            var storage = Storage.GetInstance();
            await _hubContext.Clients.All.ReceiveGrid(storage.Grid.ConvertForSending());
            foreach (var enemy in storage.Enemies)
            {
                enemy.Move(storage.State);
            }

            foreach (var state in storage.State)
            {
                int currentX = state.Value.Coordinates.X;
                int currentY = state.Value.Coordinates.Y;
                Tile desiredTile = new EmptyTile();
                switch (state.Value.Direction)
                {
                    case EnumDirection.Up:
                        desiredTile = storage.Grid.GetTile(currentX, currentY - 1);
                        if (state.Value.Coordinates.Y > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            state.Value.Coordinates = new(currentX, currentY - 1);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = _emptyTileFactory.CreateTile() as EmptyTile;
                                storage.Grid.ChangeTile(desiredTile, currentX, currentY - 1);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                    case EnumDirection.Right:
                        desiredTile = storage.Grid.GetTile(currentX + 1, currentY);
                        if (state.Value.Coordinates.X < storage.Grid.Width - 1 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(1, 0);
                            state.Value.Coordinates = new(currentX + 1, currentY);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = _emptyTileFactory.CreateTile() as EmptyTile;
                                storage.Grid.ChangeTile(desiredTile, currentX + 1, currentY);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                    case EnumDirection.Down:
                        desiredTile = storage.Grid.GetTile(currentX, currentY + 1);
                        if (state.Value.Coordinates.Y < storage.Grid.Height - 1 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(0, 1);
                            state.Value.Coordinates = new(currentX, currentY + 1);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = _emptyTileFactory.CreateTile() as EmptyTile;
                                storage.Grid.ChangeTile(desiredTile, currentX, currentY + 1);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                    case EnumDirection.Left:
                        desiredTile = storage.Grid.GetTile(currentX - 1, currentY);
                        if (state.Value.Coordinates.X > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(-1, 0);
                            state.Value.Coordinates = new(currentX - 1, currentY);
                            if (desiredTile.Type == EnumTileType.Pellet)
                            {
                                desiredTile = _emptyTileFactory.CreateTile() as EmptyTile;
                                storage.Grid.ChangeTile(desiredTile, currentX - 1, currentY);
                                state.Value.Points += 1;
                            }
                        }

                        break;
                }

                if (state.Value.Points >= 100)
                {
                    Finish();
                }
            }

            var enemyData = storage.Enemies.Select(e => new EnemyModel
            {
                Position = e.Position,
                Character = e.Character
            }).ToList();
            await _hubContext.Clients.All.ReceiveEnemies(enemyData);
            storage.Ticks += 1;
        }
    }
}