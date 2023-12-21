using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class FoodPoisonAntidoteTile : Tile
    {
        public FoodPoisonAntidoteTile()
        {
            Type = EnumTileType.FoodPoisonAntidote;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}