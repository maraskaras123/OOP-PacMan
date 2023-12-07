using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class RedGhost : GhostBase
    {
        private int _ticksSinceLastDirectionChange;
        private int _ticksTillDirectionChange = 4;
        private Point _currentDirection = Directions[0]; // Default to right

        private static readonly List<Point> Directions = new()
        {
            new(1, 0), // Right
            new(-1, 0), // Left
            new(0, 1), // Down
            new(0, -1) // Up
        };

        public RedGhost() : base('R', 1) { }


        private Point ChooseRandomDirection()
        {
            var rand = new Random();
            return Directions[rand.Next(Directions.Count)];
        }

        protected override Point FindPlayer(Dictionary<string, PlayerStateModel> playerStates)// this ghost doesnt need to find the possition of an enemy
        {
            return Point.Empty;
        }

        protected override Point MovePattern(GameStateModel session, Point start, Point end)
        {
            _ticksSinceLastDirectionChange++;

            if (_ticksSinceLastDirectionChange >= _ticksTillDirectionChange)
            {
                _currentDirection = ChooseRandomDirection();
                _ticksSinceLastDirectionChange = 0;
            }

            var nextPosition = new Point(Position.X + _currentDirection.X, Position.Y + _currentDirection.Y);

            // Sorry i changed it up, maybe i should revert back to points
            if (session.Grid.GetTile(nextPosition.X, nextPosition.Y).Type != EnumTileType.Wall &&
                CanMoveTo(session, nextPosition))
            {
                return nextPosition;
            }
            else
            {
                _currentDirection = ChooseRandomDirection();
            }
            return Position;
        }
    }
}