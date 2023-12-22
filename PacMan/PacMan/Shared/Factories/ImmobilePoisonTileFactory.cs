using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class ImmobilePoisonTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new ImmobilePoisonTile();
        }
    }
}