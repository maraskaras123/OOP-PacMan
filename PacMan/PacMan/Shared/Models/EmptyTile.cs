using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class EmptyTile : Tile
    {
        public EmptyTile()
        {
            Type = EnumTileType.Empty;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitEmptyTile();
        }
    }
}