using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public class PointsPoison : IPoison
    {
        public void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitPoisonTile(this);
        }
        public void ApplyEffect(PoisonedPacmanState player)
        {
            player.DecreasingPoints = true;
        }

        public void RemoveEffect(PoisonedPacmanState player)
        {
            player.DecreasingPoints = false;
        }
    }
}