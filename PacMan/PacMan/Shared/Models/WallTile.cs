using PacMan.Shared.Enums;
using PacMan.Shared.Models;
public class WallTile : Tile
{
    public WallTile()
    {
        this.Type = EnumTileType.Wall;
    }
}