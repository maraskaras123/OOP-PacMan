using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class WallTile : Tile
    {
        public WallTile()
        {
            Type = EnumTileType.Wall;
        }
    }
}