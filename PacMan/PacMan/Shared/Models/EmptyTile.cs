using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class EmptyTile : Tile
    {
        public EmptyTile()
        {
            Type = EnumTileType.Empty;
        }
    }
}