using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class PelletTile : Tile
    {
        public PelletTile()
        {
            Type = EnumTileType.Pellet;
        }
    }
}