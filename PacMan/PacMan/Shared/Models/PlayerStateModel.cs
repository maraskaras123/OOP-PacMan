using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class PlayerStateModel : PlayerStateBaseModel
    {
        public EnumDirection Direction { get; set; }
        public Point Coordinates { get; set; }
        public int Points { get; set; }
        private int _ticksTillNormalState = 0;
        private IPacmanState _currentState;

        public PlayerStateModel()
        {
            Direction = EnumDirection.Right;
            Coordinates = new(0, 0);
            _currentState = new NormalPacmanState(this);
        }

        public void TickToNormalState()
        {
            if(_ticksTillNormalState!=0)
            {
                _ticksTillNormalState--;
                if(_ticksTillNormalState == 0)
                {
                    _currentState = new NormalPacmanState(this);
                }
            }
        }

        public void SetToNormalTick(int ticks)
        {
            _ticksTillNormalState = ticks;
        }

        public PlayerStateModel(Point point)
        {
            Direction = EnumDirection.Right;
            Coordinates = point;
            _currentState = new NormalPacmanState(this);
        }

        public PlayerStateModel UpdateDirection(EnumDirection direction)
        {
            var model = (PlayerStateModel)MemberwiseClone();
            model.Direction = direction;
            model._currentState = _currentState.CloneForNewPlayerStateModel(model);
            return model;
        }
        public void AddPoints(int points)
        {
            Points += points;
        }
        public void CollideWithGhost()
        {
            _currentState.CollideWithGhost();
        }

        public void EatPellet()
        {
            _currentState.EatPellet();
        }
        public void SetState(IPacmanState state)
        {
            _currentState = state;
        }
    }
}