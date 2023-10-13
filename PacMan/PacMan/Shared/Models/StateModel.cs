using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public record StateModel
    {
        public EnumGameState GameState { get; }
        public List<string> Coordinates { get; }
        public List<int> Points { get; }

        public StateModel(EnumGameState gameState, List<string> coordinates, List<int> points)
        {
            GameState = gameState;
            Points = points;
            Coordinates = coordinates;
        }
    }
}