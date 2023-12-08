
namespace PacMan.Shared.Models
{
    public interface IPacmanState
    {
        void EatPellet();
        void CollideWithGhost();
        IPacmanState CloneForNewPlayerStateModel(PlayerStateModel model);
    }
}