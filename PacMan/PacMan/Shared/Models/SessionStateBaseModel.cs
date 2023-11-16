using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public record SessionStateBaseModel
    {
        public string Key { get; }
        public List<PlayerStateBaseModel> Players { get; }
        public EnumGameState GameState { get; }

        public SessionStateBaseModel(string key, List<PlayerStateBaseModel> players, EnumGameState gameState)
        {
            Key = key;
            Players = players;
            GameState = gameState;
        }
    }
}