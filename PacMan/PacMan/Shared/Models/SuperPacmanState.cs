namespace PacMan.Shared.Models
{
    public class SuperPacmanState : IPacmanState
    {
        private readonly PlayerStateModel _player;
        public SuperPacmanState(PlayerStateModel player)
        {
            _player = player;
        }

        public IPacmanState CloneForNewPlayerStateModel(PlayerStateModel model)
        {
            return new SuperPacmanState(model);
        }

        public void CollideWithGhost()
        {
            _player.AddPoints(10);
        }

        public void EatPellet()
        {
            _player.AddPoints(2);
        }
    }
}