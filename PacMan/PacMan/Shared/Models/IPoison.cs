using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public interface IPoison
    {
        void ApplyEffect(PoisonedPacmanState player);
        void RemoveEffect(PoisonedPacmanState player);
        void AcceptVisitor(IVisitor visitor);
    }
}