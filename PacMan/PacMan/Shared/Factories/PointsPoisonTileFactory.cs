using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class PointsPoisonTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new PointsPoisonTile();
        }
    }
}