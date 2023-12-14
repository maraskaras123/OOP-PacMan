namespace PacMan.Shared.Models
{
    public class SlowPoison : IPoison
    {
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