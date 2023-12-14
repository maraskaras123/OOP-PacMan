using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class FoodPoisonTile : Tile
    {
        public FoodPoisonTile()
        {
            Type = EnumTileType.FoodPoison;
        }
    }
}