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
        public bool Immobilized;
        public bool AbleToEat = true;
        public bool DecreasingPoints = false;

        public PoisonedPacmanState(PlayerStateModel player, List<IPoison> poisons,
            bool immobilized, bool ableToEat, bool decreasingPoints, int moveSpeedTick, int imobilizedTicks,
            int pelletPoints)
        {
            _poisons = poisons;
            _player = player;
            Immobilized = immobilized;
            AbleToEat = ableToEat;
            DecreasingPoints = decreasingPoints;
            MoveSpeedTick = moveSpeedTick;
            _imobilizedTicks = imobilizedTicks;
            PelletPoints = pelletPoints;
        }

        public PoisonedPacmanState(PlayerStateModel player)
        {
            _poisons = new();
            _player = player;
            Immobilized = false;
            AbleToEat = true;
            DecreasingPoints = false;
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
            if (Immobilized)
            {
                _imobilizedTicks++;
                if (_imobilizedLenght == _imobilizedTicks)
                {
                    _imobilizedTicks = 0;
                    RemoveEffect(this, typeof(ImmobilePoison));
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
            return new PoisonedPacmanState(model, _poisons, Immobilized, AbleToEat, DecreasingPoints, MoveSpeedTick,
                _imobilizedTicks, PelletPoints);
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
            if (DecreasingPoints)
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