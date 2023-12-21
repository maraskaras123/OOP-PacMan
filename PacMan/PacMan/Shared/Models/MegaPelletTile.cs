using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class MegaPelletTile : Tile
    {
        public MegaPelletTile()
        {
            Type = EnumTileType.MegaPellet;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitMegaPelletTile();
        }
    }
}