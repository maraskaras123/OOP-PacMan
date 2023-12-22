using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class ImmobilePoison : IPoison
    {
        public void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(this);
        }
        public void ApplyEffect(PoisonedPacmanState player)
        {
            player.Immobilized = true;
        }

        public void RemoveEffect(PoisonedPacmanState player)
        {
            player.Immobilized = false;
        }
    }
}