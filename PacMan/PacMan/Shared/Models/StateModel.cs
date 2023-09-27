using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public record StateModel
    {
        public EnumGameState GameState { get; }
        public List<string> Coordinates { get; }

        public StateModel(EnumGameState gameState, List<string> coordinates)
        {
            GameState = gameState;
            Coordinates = coordinates;
        }
    }
}