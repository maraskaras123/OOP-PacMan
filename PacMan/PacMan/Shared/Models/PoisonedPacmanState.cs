namespace PacMan.Shared.Models
{
    public class PoisonedPacmanState : IPacmanState
    {
        public int PelletPoints;
        private int _imobilizedLenght = 6;
        private int _imobilizedTicks = 0;
        private List<IPoison> _poisons;
        public int MoveSpeedTick;
        private readonly PlayerStateModel _player;
        public bool Imobilized;
        public bool AbleToEat = true;
        public bool DecresingPoints = false;
        public PoisonedPacmanState(PlayerStateModel player, List<IPoison> poisons,
         bool imobilized, bool ableToEat, bool decresingPoints, int moveSpeedTick, int imobilizedTicks,
         int pelletPoints)
        {
            _poisons = poisons;
            _player = player;
            Imobilized = imobilized;
            AbleToEat = ableToEat;
            DecresingPoints = decresingPoints;
            MoveSpeedTick = moveSpeedTick;
            _imobilizedTicks = imobilizedTicks;
            PelletPoints = pelletPoints;
        }
        public PoisonedPacmanState(PlayerStateModel player)
        {
            _poisons = new List<IPoison>();
            _player = player;
            Imobilized = false;
            AbleToEat = true;
            DecresingPoints = false;
            MoveSpeedTick = 1;
            PelletPoints = 1;
        }

        public void AddPoison(IPoison poison, PoisonedPacmanState player)
        {
            if (!_poisons.Any(p => p.GetType() == poison.GetType())) // checks if the poison isnt already active
            {

                _poisons.Add(poison);
                poison.ApplyEffect(player);
            }
        }
        public void RemoveEffect(PoisonedPacmanState player, Type removePoison)
        {
            IPoison poisonToRemove = null;
            foreach (var poison in _poisons)
            {
                if (removePoison == poison.GetType())
                {
                    poisonToRemove = poison;
                    break;
                }
            }
            if (poisonToRemove != null)
            {
                poisonToRemove.RemoveEffect(player);
                _poisons.Remove(poisonToRemove);
            }
        }

        public bool CanMove()
        {
            if (Imobilized)
            {
                _imobilizedTicks++;
                if (_imobilizedLenght == _imobilizedTicks)
                {
                    _imobilizedTicks = 0;
                    RemoveEffect(this, typeof(ImobilePoison));
                }
                return false;
            }
            _player.TicksTillMove--;
            if (_player.TicksTillMove == 0)
            {
                _player.TicksTillMove = MoveSpeedTick;
                return true;
            }
            return false;
        }

        public IPacmanState CloneForNewPlayerStateModel(PlayerStateModel model)
        {
            return new PoisonedPacmanState(model, _poisons, Imobilized, AbleToEat, DecresingPoints, MoveSpeedTick, _imobilizedTicks, PelletPoints);
        }

        public void CollideWithGhost()
        {
            _player.AddPoints(-10);
        }

        public void EatPellet()
        {
            _player.AddPoints(PelletPoints);
        }

        public void Tick()
        {
            if (DecresingPoints)
            {
                _player.AddPoints(-1);
            }
        }

        public bool CanEat()
        {
            return AbleToEat;
        }
    }
}