using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class GameStateModel
    {
        public EnumGameState GameState { get; set; } = EnumGameState.Initializing;
        public List<IEnemy> Enemies = new();
        public TileGrid Grid { get; set; } = new();
        public Dictionary<string, string> Connections { get; set; } = new();
        public Dictionary<string, PlayerStateModel> State { get; set; } = new();

        public int Ticks { get; set; }
    }
}