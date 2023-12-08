namespace PacMan.Shared.Models
{
    public class NormalPacmanState : IPacmanState
    {
        private readonly PlayerStateModel _player;
        public NormalPacmanState(PlayerStateModel player)
        {
            _player = player;
        }

        public IPacmanState CloneForNewPlayerStateModel(PlayerStateModel model)
        {
            return new NormalPacmanState(model);
        }

        public void CollideWithGhost()
        {
            _player.AddPoints(-10);
        }

        public void EatPellet()
        {
            _player.AddPoints(1);
        }
    }
}