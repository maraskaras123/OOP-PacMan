namespace PacMan.Shared.Models
{
    public class NormalPacmanState : IPacmanState
    {
        private int _moveSpeedTick = 1;
        private readonly PlayerStateModel _player;
        public NormalPacmanState(PlayerStateModel player)
        {
            _player = player;
        }

        public bool CanMove()
        {
            {
                _player.TicksTillMove--;
                if (_player.TicksTillMove == 0)
                {
                    _player.TicksTillMove = _moveSpeedTick;
                    return true;
                }
                return false;
            }
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

        public void Tick()
        {

        }

        public bool CanEat()
        {
            return true;
        }
    }
}