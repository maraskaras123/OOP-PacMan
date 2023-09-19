using Microsoft.AspNetCore.SignalR;
using PacMan.Server.Hubs;
using PacMan.Shared;
using PacMan.Shared.Enums;

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
                Storage.ConnectionIds = new();
            }
        }

        public void Start()
        {
            Storage.GameState = EnumGameState.Starting;
        }

        public async Task Init()
        {
            Storage.GameState = EnumGameState.Running;
            while (Storage.GameState != EnumGameState.Finished)
            {
                await Task.WhenAll(Task.Delay(100), Tick());
                await _hubContext.Clients.All.Tick();
            }
        }

        public void Finish()
        {
            Storage.GameState = EnumGameState.Finished;
        }

        // Game Logic
        private async Task<int> Tick()
        {
            
        }
    }
}