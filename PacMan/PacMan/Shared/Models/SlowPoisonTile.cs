using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class SlowPoisonTile : Tile
    {
        public SlowPoisonTile()
        {
            Type = EnumTileType.SlowPoison;
        }
    }
}