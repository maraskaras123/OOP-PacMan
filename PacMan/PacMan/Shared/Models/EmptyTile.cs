
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

public class EmptyTile : Tile
{
    public EmptyTile()
    {
        this.Type = EnumTileType.Empty;
    }
}