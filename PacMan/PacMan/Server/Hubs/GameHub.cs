using System.Drawing;
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
        Task Starting(EnumGameState gameState);
        Task Tick(StateModel state);
        Task ReceiveWalls(List<Point> walls);

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
        public async Task SendWalls()
        {
            await Clients.Caller.ReceiveWalls(Storage.Walls);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Storage.ConnectionIds.RemoveAt(Storage.ConnectionIds.FindIndex(s => s == Context.ConnectionId));
            Storage.State.Remove(Context.ConnectionId);
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
            await Clients.All.Starting(EnumGameState.Starting); // i dont think this did anything before?
            await Task.Delay(1000);
            Task.Run(_gameService.Init);
        }

        [HubMethodName("OnChangeDirection")]
        public async Task ChangeDirectionAsync(EnumDirection direction)
        {
            //_gameService.ChangeDirectionAsync(direction, Context.ConnectionId);
            if (!Enum.IsDefined<EnumDirection>(direction))
            {
                throw new ArgumentException();
            }

            Storage.State[Context.ConnectionId] = new() { Direction = direction, Coordinates = Storage.State[Context.ConnectionId].Coordinates };
        }
    }
}