using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public class FoodPoisonFactory : TileFactory
    {
        public override Tile CreateTile()
        {
            return new FoodPoisonTile();
        }
    }
}