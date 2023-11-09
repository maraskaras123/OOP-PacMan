using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class RedGhost : IEnemy
    {
        public char Character => 'R';
        public Point Position { get; set; }
        private int _ticksSinceLastDirectionChange;
        private Point _currentDirection = Directions[0]; // Default to right

        private static readonly List<Point> Directions = new()
        {
            new(1, 0), // Right
            new(-1, 0), // Left
            new(0, 1), // Down
            new(0, -1) // Up
        };

        public void Move(Dictionary<string, GameStateModel> playerStates)
        {
            _ticksSinceLastDirectionChange++;
            var storage = Storage.GetInstance();

            if (_ticksSinceLastDirectionChange >= 4)
            {
                _currentDirection = ChooseRandomDirection();
                _ticksSinceLastDirectionChange = 0;
            }

            var nextPosition = new Point(Position.X + _currentDirection.X, Position.Y + _currentDirection.Y);

            // Assuming Storage.Walls is accessible from this scope
            // Sorry i changed it up, maybe i should revert back to points
            if (storage.Grid.GetTile(nextPosition.X, nextPosition.Y).Type != EnumTileType.Wall &&
                CanMoveTo(nextPosition))
            {
                Position = nextPosition;
            }
            else
            {
                _currentDirection = ChooseRandomDirection();
            }
        }

        private bool CanMoveTo(Point nextPosition)
        {
            var storage = Storage.GetInstance();
            // Check out-of-bounds conditions
            if (nextPosition.X < 0 || nextPosition.X > 30 || nextPosition.Y < 0 || nextPosition.Y > 30)
            {
                return false;
            }

            // Check if the next position is a wall
            if (storage.Grid.GetTile(nextPosition.X, nextPosition.Y).Type == EnumTileType.Wall)
            {
                return false;
            }

            return true;
        }

        private Point ChooseRandomDirection()
        {
            var rand = new Random();
            return Directions[rand.Next(Directions.Count)];
        }
    }
}