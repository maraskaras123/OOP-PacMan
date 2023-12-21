using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class ImobilePoisonTile : Tile
    {
        public ImobilePoisonTile()
        {
            Type = EnumTileType.ImobilePoison;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(new ImobilePoison());
        }
    }
}