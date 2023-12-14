using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class AllCureTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new AllCureTile();
        }
    }
}