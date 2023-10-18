using Microsoft.AspNetCore.SignalR;
using PacMan.Server.Hubs;
using PacMan.Shared;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Server.Services
{
    public interface IGameService
    {
        void Reset(bool? clearPlayers);
        void Start();
        Task Init();
        void Finish();
    }

    public class GameService : IGameService
    {
        private readonly IHubContext<GameHub, IGameHubClient> _hubContext;

        public GameService(IHubContext<GameHub, IGameHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        public void Reset(bool? clearPlayers)
        {
            Storage.GameState = EnumGameState.Initializing;
            if (clearPlayers ?? false)
            {
                Storage.ConnectionIds = new ();
            }
        }

        public void Start()
        {
            InitializeWalls();
            Storage.GameState = EnumGameState.Starting;
        }

        private void InitializeWalls()
        {
            var rnd = new Random();
            for (int i = 0; i < 31; i++)
            {
                for (int j = 0; j < 31; j++)
                {

                    Storage.Tiles.Add($"{i}_{j}",new Tile(EnumTileType.Pellet));
                }
            }
            for (var i = 0; i < 50; i++)
            {
                int r1 = rnd.Next(0, 30);
                int r2 = rnd.Next(0, 30);
                var tile = GetTile(r1, r2);
                if (tile != null)
                {
                    tile.Type = EnumTileType.Wall;
                }

                //Storage.Walls.Add(new (rnd.Next(0, 30), rnd.Next(0, 30)));
            }

        }

        public async Task Init()
        {
            Storage.GameState = EnumGameState.Running;
            Storage.Ticks = 0;

            foreach (var connectionId in Storage.ConnectionIds)
            {
                var stateModel = new GameStateModel();
                Storage.State.Add(connectionId, stateModel);


                await _hubContext.Clients.All.ReceiveTiles(Storage.Tiles);

            }

            while (Storage.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(400), Tick());
                await _hubContext.Clients.All.ReceiveTiles(Storage.Tiles);
                await _hubContext.Clients.All.Tick(new (Storage.GameState,
                    Storage.State.Select((x, index) => $"{index},{x.Value.Coordinates.X},{x.Value.Coordinates.Y}")
                        .ToList(),
                    Storage.State.Select(x => x.Value.Points).ToList()));
            }
        }

        public void Finish()
        {
            Storage.GameState = EnumGameState.Finished;
        }

        private Tile GetTile(int x, int y)
        {
            if (Storage.Tiles.TryGetValue($"{x}_{y}", out Tile tile))
            {
                return tile;
            }
            return new Tile();
        }

        // Game Logic
        private async Task Tick()
        {
            foreach (var state in Storage.State)
            {
                int currentX = state.Value.Coordinates.X;
                int currentY = state.Value.Coordinates.Y;
                Tile desiredTile = new Tile();
                switch (state.Value.Direction)
                {
                    case EnumDirection.Up:
                        desiredTile = GetTile(currentX, currentY - 1);
                        if (state.Value.Coordinates.Y > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(0, -1); this doesnt update the initial value
                            state.Value.Coordinates = new (state.Value.Coordinates.X, state.Value.Coordinates.Y - 1);
                        }
                        break;
                    case EnumDirection.Right:
                        desiredTile = GetTile(currentX + 1, currentY);
                        if (state.Value.Coordinates.X < 30 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(1, 0);
                            state.Value.Coordinates = new (state.Value.Coordinates.X + 1, state.Value.Coordinates.Y);
                        }
                        break;
                    case EnumDirection.Down:
                        desiredTile = GetTile(currentX, currentY + 1);
                        if (state.Value.Coordinates.Y < 30 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(0, 1);
                            state.Value.Coordinates = new (state.Value.Coordinates.X, state.Value.Coordinates.Y + 1);
                        }
                        break;
                    case EnumDirection.Left:
                        desiredTile = GetTile(currentX - 1, currentY);
                        if (state.Value.Coordinates.X > 0 &&
                            desiredTile.Type != EnumTileType.Wall)
                        {
                            //state.Value.Coordinates.Offset(-1, 0);
                            state.Value.Coordinates = new (state.Value.Coordinates.X - 1, state.Value.Coordinates.Y);
                        }
                        break;
                }
                if (desiredTile.Type == EnumTileType.Pellet)
                {
                    desiredTile.SetToEmpty();
                    state.Value.Points += 1;
                }

                /*
                if (Storage.Ticks % 10 == 9)
                {
                    state.Value.Points += 1;
                }*/
            }

            Storage.Ticks += 1;
            
        }
    }
}