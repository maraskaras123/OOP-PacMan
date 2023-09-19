using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Shared
{
    public static class Storage
    {
        public static EnumGameState GameState { get; set; } = EnumGameState.Initializing;
        public static List<string> ConnectionIds { get; set; } = new();
        public static StateModel State { get; set; } = new ();
    }
}