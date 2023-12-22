using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class SlowPoisonAntidoteFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new SlowPoisonAntidoteTile();
        }
    }
}