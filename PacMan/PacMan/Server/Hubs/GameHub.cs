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
        Task ReceiveGrid(TileGrid grid);
        Task ReceiveEnemies(List<EnemyModel> enemies);
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
            Storage.State.Remove(Context.ConnectionId);
            if (!Storage.ConnectionIds.Any())
            {
                _gameService.Finish();
            }
            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("OnStart")]
        public async Task OnStartAsync(TileGridBuilderOptions gridOptions)
        {
            _gameService.Start(gridOptions);
            await Clients.All.Starting(EnumGameState.Starting);
            await SendGrid();
            await SendEnemies();
            await Task.Delay(200);
            Task.Run(_gameService.Init);
        }

        [HubMethodName("OnChangeDirection")]
        public async Task ChangeDirectionAsync(EnumDirection direction)
        {
            if (!Enum.IsDefined<EnumDirection>(direction))
            {
                throw new ArgumentException();
            }

            Storage.State[Context.ConnectionId] = new() 
            { 
                Direction = direction,
                Coordinates = Storage.State[Context.ConnectionId].Coordinates, 
                Points = Storage.State[Context.ConnectionId].Points 
            };
        }

        public async Task SendGrid()
        {
            await Clients.Caller.ReceiveGrid(Storage.Grid);
        }
        
        public async Task SendEnemies()
        {
            var enemyData = Storage.Enemies.Select(e => new EnemyModel
            {
                Position = e.Position,
                Character = e.Character
            }).ToList();

            await Clients.Caller.ReceiveEnemies(enemyData);
        }
    }
}