using PacMan.Shared.Helpers;
using PacMan.Shared.Models;

namespace PacMan.Shared
{
    public sealed class Storage
    {
        private static readonly Storage Instance = new();

        private Storage()
        {
        }

        public static Storage GetInstance()
        {
            return Instance;
        }

        private readonly Dictionary<string, GameStateModel> _sessions = new();

        public string CreateSession()
        {
            var key = RandomGenerator.GenerateRandomText(8);
            var state = new GameStateModel();
            _sessions.Add(key, state);
            return key;
        }

        public void RemoveSession(string key)
        {
            _sessions.Remove(key);
        }

        public GameStateModel? GetSession(string key)
        {
            return _sessions.TryGetValue(key, out var session) ? session : null;
        }

        public List<SessionStateBaseModel> GetSessionList()
        {
            return _sessions
                .Select(x => new SessionStateBaseModel(x.Key,
                    x.Value.State.Select(s => new PlayerStateBaseModel { Name = s.Value.Name }).ToList(),
                    x.Value.GameState))
                .ToList();
        }

        public KeyValuePair<string, GameStateModel>? FindSession(string connectionId)
        {
            return _sessions.FirstOrDefault(x => x.Value.Connections.ContainsKey(connectionId));
        }
    }
}