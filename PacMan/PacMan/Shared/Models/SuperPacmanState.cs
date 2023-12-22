namespace PacMan.Shared.Models
{
    public class SuperPacmanState : IPacmanState
    {
        private int _ticksTillNormalState; // how long this state will last
        private int _moveSpeedTick = 1;
        private readonly PlayerStateModel _player;
        public SuperPacmanState(PlayerStateModel player, int ticksTillNormalState)
        {
            _player = player;
            _player.TicksTillMove = _moveSpeedTick;
            _ticksTillNormalState = ticksTillNormalState;
        }

        public IPacmanState CloneForNewPlayerStateModel(PlayerStateModel model)
        {
            return new SuperPacmanState(model, _ticksTillNormalState);
        }

        public void CollideWithGhost()
        {
            _player.AddPoints(10);
        }

        public void EatPellet()
        {
            _player.AddPoints(2);
        }

        public void Tick()
        {
            _ticksTillNormalState--;
            if(_ticksTillNormalState == 0)
            {
                _player.SetState(new NormalPacmanState(_player));
            }
        }

        public bool CanMove()
        {
            _player.TicksTillMove--;
            if(_player.TicksTillMove == 0)
            {
                _player.TicksTillMove = _moveSpeedTick;
                return true;
            }
            return false;
        }

        public bool CanEat()
        {
            return true;
        }
    }
}