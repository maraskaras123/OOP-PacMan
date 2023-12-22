using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class SlowPoisonTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new SlowPoisonTile();
        }
    }
}