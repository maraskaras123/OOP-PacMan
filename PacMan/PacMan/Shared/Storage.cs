using PacMan.Shared.Enums;

namespace PacMan.Shared
{
    public static class Storage
    {
        public static EnumGameState State { get; set; } = EnumGameState.Initializing;
        public static List<string> UserIds { get; set; } = new List<string>();
    }
}