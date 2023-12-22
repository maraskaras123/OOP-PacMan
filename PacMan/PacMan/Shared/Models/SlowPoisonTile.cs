using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class SlowPoisonTile : Tile
    {
        public SlowPoisonTile()
        {
            Type = EnumTileType.SlowPoison;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(new SlowPoison());
        }
    }
}