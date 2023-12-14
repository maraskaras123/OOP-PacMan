using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class PlayerStateModel : PlayerStateBaseModel
    {
        public EnumDirection Direction { get; set; }
        public Point Coordinates { get; set; }
        public int Points { get; set; }

        
        public int TicksTillMove = 1;
        private IPacmanState _currentState;

        public PlayerStateModel()
        {
            Direction = EnumDirection.Right;
            Coordinates = new(0, 0);
            _currentState = new NormalPacmanState(this);
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
        public void Tick()
        {
            _currentState.Tick();
            
        }
        public bool CanMove()
        {
            return _currentState.CanMove();
        }
        public Type GetState()
        {
            return _currentState.GetType();
        }
        public void AddPoison(IPoison poison)
        {
            if(_currentState.GetType() != typeof(PoisonedPacmanState))
            {
                SetState(new PoisonedPacmanState(this));
            }
            ((PoisonedPacmanState)_currentState).AddPoison(poison, (PoisonedPacmanState)_currentState);
        }
        public void RemovePoison(Type poison)
        {
            if(_currentState.GetType() == typeof(PoisonedPacmanState))
            {
                ((PoisonedPacmanState)_currentState).RemoveEffect((PoisonedPacmanState)_currentState, poison);
            }
        }
        public bool CanEat()
        {
            return _currentState.CanEat();
        }
    }
}