using System.Drawing;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Shared
{
    public sealed class Storage
    {
        private Storage()
        {
        }

        private static readonly Storage Instance = new();

        public static Storage GetInstance()
        {
            return Instance;
        }

        public EnumGameState GameState { get; set; } = EnumGameState.Initializing;
        public List<IEnemy> Enemies = new();
        public TileGrid Grid { get; set; } = new();
        public List<string> ConnectionIds { get; set; } = new();
        public Dictionary<string, GameStateModel> State { get; set; } = new();

        public int Ticks { get; set; }
    }
}