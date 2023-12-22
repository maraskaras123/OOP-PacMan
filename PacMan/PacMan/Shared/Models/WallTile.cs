using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class WallTile : Tile
    {
        public WallTile()
        {
            Type = EnumTileType.Wall;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitWallTile();
        }
    }
}