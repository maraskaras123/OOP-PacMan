using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class FoodPoisonAntidoteFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new FoodPoisonAntidoteTile();
        }
    }
}