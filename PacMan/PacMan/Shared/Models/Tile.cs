using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public abstract class Tile
    {
        public EnumTileType Type { get; set; }
    }
}