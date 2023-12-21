using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class SlowPoison : IPoison
    {
        public void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(this);
        }

        public void ApplyEffect(PoisonedPacmanState player)
        {
            player.MoveSpeedTick = 2;
        }

        public void RemoveEffect(PoisonedPacmanState player)
        {
            player.MoveSpeedTick = 1;
        }
    }
}