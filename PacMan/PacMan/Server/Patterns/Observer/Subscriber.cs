using PacMan.Server.Services;
using PacMan.Shared.Patterns.Observer;

namespace PacMan.Server.Patterns.Observer
{
    public class Subscriber : ISubscriber
    {
        private readonly IGameService _gameService;
        private readonly string _connectionId;

        public Subscriber(IGameService gameService, string connectionId)
        {
            _gameService = gameService;
            _connectionId = connectionId;
        }

        public async Task Notify(bool add, string name)
        {
            await _gameService.PlayerUpdate(_connectionId, add, name);
        }
    }
}