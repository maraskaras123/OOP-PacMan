using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class PelletTile : Tile
    {
        public PelletTile()
        {
            Type = EnumTileType.Pellet;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPelletTile();
        }
    }
}