using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class AllCureTile : Tile
    {
        public AllCureTile()
        {
            Type = EnumTileType.AllCureTile;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitAllCureTile();
        }
    }
}