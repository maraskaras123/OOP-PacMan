using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class PointPoisonAntidoteFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new PointsPoisonAntidoteTile();
        }
    }
}