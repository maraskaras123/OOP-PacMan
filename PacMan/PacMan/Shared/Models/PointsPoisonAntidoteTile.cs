using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class PointsPoisonAntidoteTile : Tile
    {
        public PointsPoisonAntidoteTile()
        {
            Type = EnumTileType.PointsPoisonAntidote;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}