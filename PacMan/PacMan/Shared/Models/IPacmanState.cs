
namespace PacMan.Shared.Models
{
    public interface IPacmanState
    {
        void EatPellet();
        void CollideWithGhost();
        void Tick();
        public bool CanMove();
        public bool CanEat();
        IPacmanState CloneForNewPlayerStateModel(PlayerStateModel model);
    }
}