using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class PointsPoisonTile : Tile
    {
        public PointsPoisonTile()
        {
            Type = EnumTileType.PointsPoison;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(new PointsPoison());
        }
    }
}