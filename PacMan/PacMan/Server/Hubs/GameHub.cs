using Microsoft.AspNetCore.SignalR;
using PacMan.Server.Services;
using PacMan.Shared;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Server.Hubs
{
    public interface IGameHubClient
    {
        Task RegisteredUserId(int userId);
        Task Starting();
        Task Tick(StateModel state);
        
    }

    public class GameHub : Hub<IGameHubClient>
    {
        private IGameService _gameService;

        public GameHub(IGameService gameService)
        {
            _gameService = gameService;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.RegisteredUserId(Storage.ConnectionIds.Count);
            Storage.ConnectionIds.Add(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Storage.ConnectionIds.RemoveAt(Storage.ConnectionIds.FindIndex(s => s == Context.ConnectionId));
            if (!Storage.ConnectionIds.Any())
            {
                _gameService.Finish();
            }
            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("OnStart")]
        public async Task OnStartAsync()
        {
            _gameService.Start();
            await Clients.All.Starting();
            await Task.Delay(5000);
            await _gameService.Init();
        }

        [HubMethodName("OnChangeDirection")]
        public async Task ChangeDirectionAsync(EnumDirection direction)
        {
            //var user = Storage.ConnectionIds.FirstOrDefault(s => s == Context.ConnectionId);
            if (!Storage.ConnectionIds.Contains(Context.ConnectionId))
            {
                throw new ArgumentException();
            }
            
            if (!Enum.IsDefined<EnumDirection>(direction))
            {
                throw new ArgumentException();
            }

            Storage.State[Context.ConnectionId].Direction = direction;

        }
    }
}