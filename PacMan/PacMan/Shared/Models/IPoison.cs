namespace PacMan.Shared.Models
{
    public interface IPoison
    {
        void ApplyEffect(PoisonedPacmanState player);
        void RemoveEffect(PoisonedPacmanState player);
    }
}