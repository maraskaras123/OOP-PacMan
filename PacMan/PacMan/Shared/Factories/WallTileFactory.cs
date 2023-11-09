
using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class WallTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new WallTile();
        }
    }
}