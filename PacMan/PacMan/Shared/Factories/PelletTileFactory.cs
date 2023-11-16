using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class PelletTileFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new PelletTile();
        }
    }
}