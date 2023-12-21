using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class FoodPoisonTile : Tile
    {
        public FoodPoisonTile()
        {
            Type = EnumTileType.FoodPoison;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(new FoodPoison());
        }
    }
}