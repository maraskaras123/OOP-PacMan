using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class SlowPoisonAntidoteTile : Tile
    {
        public SlowPoisonAntidoteTile()
        {
            Type = EnumTileType.SlowPoisonAntidote;
        }

        public override void AcceptVisitor(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}