using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class ImobilePoisonTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new ImobilePoisonTile();
        }
    }
}