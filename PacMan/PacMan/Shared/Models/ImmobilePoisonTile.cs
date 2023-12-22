using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class ImmobilePoisonTile : Tile
    {
        public ImmobilePoisonTile()
        {
            Type = EnumTileType.ImobilePoison;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(new ImmobilePoison());
        }
    }
}