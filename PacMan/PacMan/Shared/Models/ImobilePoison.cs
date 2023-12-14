namespace PacMan.Shared.Models
{
    public class ImobilePoison : IPoison
    {
        public void ApplyEffect(PoisonedPacmanState player)
        {
            player.Imobilized = true;
        }

        public void RemoveEffect(PoisonedPacmanState player)
        {
            player.Imobilized = false;
        }
    }
}