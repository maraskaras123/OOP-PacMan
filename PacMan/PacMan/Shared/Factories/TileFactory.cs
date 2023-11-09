
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

public abstract class TileFactory
{
    public abstract Tile CreateTile();
    public virtual Tile ConvertToEmpty()
    {
        return new EmptyTile();
    }
}
