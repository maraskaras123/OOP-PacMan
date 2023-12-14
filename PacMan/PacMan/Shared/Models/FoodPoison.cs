namespace PacMan.Shared.Models
{
    public class FoodPoison : IPoison
    {
        public void ApplyEffect(PoisonedPacmanState player)
        {
            player.AbleToEat = false;
            player.PelletPoints = 0;
        }

        public void RemoveEffect(PoisonedPacmanState player)
        {
            player.AbleToEat = true;
            player.PelletPoints = 1;
        }
    }
}