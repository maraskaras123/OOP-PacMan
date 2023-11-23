using PacMan.Shared.Enums;
using PacMan.Shared.Observer;

namespace PacMan.Shared.Models
{
    public class GameStateModel
    {
        public EnumGameState GameState { get; set; } = EnumGameState.Initializing;
        public List<IEnemy> Enemies { get; init; } = new();
        public TileGrid Grid { get; set; } = new();
        public Dictionary<string, string> Connections { get; init; } = new();
        public Dictionary<string, PlayerStateModel> State { get; init; } = new();
        public Publisher Publisher { get; init; } = new();

        public int Ticks { get; set; }
    }
}