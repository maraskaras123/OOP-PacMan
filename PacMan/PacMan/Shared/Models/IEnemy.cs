using System.Drawing;

namespace PacMan.Shared.Models
{
    public interface IEnemy
    {
        Point Position { get; set; }
        char Character { get; }
        void Move(GameStateModel session);
        void Respawn(GameStateModel session);
    }
}