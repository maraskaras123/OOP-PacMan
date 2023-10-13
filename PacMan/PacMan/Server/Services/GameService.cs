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
            for (var i = 0; i < 50; i++)
            {
                Storage.Walls.Add(new (rnd.Next(0, 30), rnd.Next(0, 30)));
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


                await _hubContext.Clients.All.ReceiveWalls(Storage.Walls);

            }

            while (Storage.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(1000), Tick());
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

        // Game Logic
        private async Task Tick()
        {
            foreach (var state in Storage.State)
            {
                switch (state.Value.Direction)
                {
                    case EnumDirection.Up:
                        if (state.Value.Coordinates.Y > 0 &&
                            !Storage.Walls.Contains(new (state.Value.Coordinates.X, state.Value.Coordinates.Y - 1)))
                        {
                            //state.Value.Coordinates.Offset(0, -1); this doesnt update the initial value
                            state.Value.Coordinates = new (state.Value.Coordinates.X, state.Value.Coordinates.Y - 1);
                        }

                        break;
                    case EnumDirection.Right:
                        if (state.Value.Coordinates.X < 30 &&
                            !Storage.Walls.Contains(new (state.Value.Coordinates.X + 1, state.Value.Coordinates.Y)))
                        {
                            //state.Value.Coordinates.Offset(1, 0);
                            state.Value.Coordinates = new (state.Value.Coordinates.X + 1, state.Value.Coordinates.Y);
                        }

                        break;
                    case EnumDirection.Down:
                        if (state.Value.Coordinates.Y < 30 &&
                            !Storage.Walls.Contains(new (state.Value.Coordinates.X, state.Value.Coordinates.Y + 1)))
                        {
                            //state.Value.Coordinates.Offset(0, 1);
                            state.Value.Coordinates = new (state.Value.Coordinates.X, state.Value.Coordinates.Y + 1);
                        }

                        break;
                    case EnumDirection.Left:
                        if (state.Value.Coordinates.X > 0 &&
                            !Storage.Walls.Contains(new (state.Value.Coordinates.X - 1, state.Value.Coordinates.Y)))
                        {
                            //state.Value.Coordinates.Offset(-1, 0);
                            state.Value.Coordinates = new (state.Value.Coordinates.X - 1, state.Value.Coordinates.Y);
                        }

                        break;
                }

                if (Storage.Ticks % 10 == 9)
                {
                    state.Value.Points += 1;
                }
            }

            Storage.Ticks += 1;
            
        }
    }
}