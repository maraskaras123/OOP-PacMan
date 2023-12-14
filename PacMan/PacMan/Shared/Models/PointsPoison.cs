namespace PacMan.Shared.Models
{
    public class PointsPoison : IPoison
    {
        public void ApplyEffect(PoisonedPacmanState player)
        {
            player.DecresingPoints = true;
        }

        public void RemoveEffect(PoisonedPacmanState player)
        {
            player.DecresingPoints = false;
        }
    }
}