
using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class MegaPelletTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new MegaPelletTile();
        }
    }
}