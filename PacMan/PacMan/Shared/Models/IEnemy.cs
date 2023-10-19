using System.Drawing;

namespace PacMan.Shared.Models
{
    public interface IEnemy
    {
        Point Position { get; set; }
        char Character { get; }
        void Move(Dictionary<string, GameStateModel> playerStates);
        
    }
}
