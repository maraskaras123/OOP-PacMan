using Microsoft.AspNetCore.SignalR;
using PacMan.Server.Services;
using PacMan.Shared;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Server.Hubs
{
    public interface IGameHubClient
    {
        Task JoinRejected();
        Task Joined(List<PlayerStateBaseModel> sessions);
        Task RegisteredUserId(int userId);
        Task StateChange(EnumGameState gameState);
        Task Tick(StateModel state);
        Task ReceiveGrid(GridModel grid);
        Task ReceiveEnemies(List<EnemyModel> enemies);
    }

    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IGameService _gameService;

        public GameHub(IGameService gameService)
        {
            _gameService = gameService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var storage = Storage.GetInstance();
            var session = storage.FindSession(Context.ConnectionId);
            if (session is not null)
            {
                session.Value.Value.Connections.Remove(Context.ConnectionId);
                if (!session.Value.Value.Connections.Any())
                {
                    storage.RemoveSession(session.Value.Key);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("JoinSession")]
        public async Task JoinSessionAsync(string sessionId, string name)
        {
            var storage = Storage.GetInstance();
            var session = storage.GetSession(sessionId);
            if (session is null || session.GameState != EnumGameState.Initializing)
            {
                await Clients.Caller.JoinRejected();
                return;
            }

            session.Connections.Add(Context.ConnectionId, name);
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            await Clients.Group(sessionId)
                .Joined(session.State.Select(x => new PlayerStateBaseModel { Name = x.Value.Name }).ToList());
        }

        [HubMethodName("OnStart")]
        public async Task OnStartAsync(TileGridBuilderOptions gridOptions)
        {
            var storage = Storage.GetInstance();
            var session = storage.FindSession(Context.ConnectionId) ?? throw new InvalidOperationException();
            _gameService.Start(session.Value, gridOptions);
            await Clients.Group(session.Key).StateChange(EnumGameState.Starting);
            await Clients.Group(session.Key).ReceiveGrid(session.Value.Grid.ConvertForSending());
            await Clients.Group(session.Key).ReceiveEnemies(session.Value.Enemies
                .Select(e => new EnemyModel { Position = e.Position, Character = e.Character })
                .ToList());
            await Task.Delay(200);
            Task.Run(() => _gameService.Init(session.Key, session.Value));
        }

        [HubMethodName("OnRestart")]
        public async Task OnRestartAsync()
        {
            var session = Storage.GetInstance().FindSession(Context.ConnectionId) ??
                          throw new InvalidOperationException();
            _gameService.Reset(session.Value);
            await Clients.Group(session.Key).StateChange(EnumGameState.Initializing);
        }

        [HubMethodName("OnChangeDirection")]
        public async Task ChangeDirectionAsync(EnumDirection direction)
        {
            if (!Enum.IsDefined(direction))
            {
                throw new ArgumentException("Direction invalid");
            }

            var storage = Storage.GetInstance();
            var session = storage.FindSession(Context.ConnectionId) ??
                          throw new InvalidOperationException("Client not connected");

            session.Value.State[Context.ConnectionId] =
                session.Value.State[Context.ConnectionId].UpdateDirection(direction);
        }
    }
}