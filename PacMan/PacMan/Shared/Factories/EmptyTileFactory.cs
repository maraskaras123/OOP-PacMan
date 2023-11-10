
using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class EmptyTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new EmptyTile();
        }
    }
}