using System.Drawing;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Shared
{
    public static class Storage
    {
        public static EnumGameState GameState { get; set; } = EnumGameState.Initializing;

        public static TileGrid Grid { get; set; } = new ();

        public static List<string> ConnectionIds { get; set; } = new ();
        public static Dictionary<string, GameStateModel> State { get; set; } = new ();

        public static int Ticks { get; set; }
    }
}