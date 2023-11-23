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

        public void Move(GameStateModel session)
        {
            _ticksSinceLastDirectionChange++;

            if (_ticksSinceLastDirectionChange >= 4)
            {
                _currentDirection = ChooseRandomDirection();
                _ticksSinceLastDirectionChange = 0;
            }

            var nextPosition = new Point(Position.X + _currentDirection.X, Position.Y + _currentDirection.Y);

            // Assuming Storage.Walls is accessible from this scope
            // Sorry i changed it up, maybe i should revert back to points
            if (session.Grid.GetTile(nextPosition.X, nextPosition.Y).Type != EnumTileType.Wall &&
                CanMoveTo(session, nextPosition))
            {
                Position = nextPosition;
            }
            else
            {
                _currentDirection = ChooseRandomDirection();
            }
        }

        private bool CanMoveTo(GameStateModel session, Point nextPosition)
        {
            
            foreach( var enemy in session.Enemies)
            {
                if(nextPosition == enemy.Position)
                {
                    return false;
                }
            }

            // Check if the next position is a wall
            return session.Grid.GetTile(nextPosition.X, nextPosition.Y).Type != EnumTileType.Wall;
        }

        private static Point ChooseRandomDirection()
        {
            var rand = new Random();
            return Directions[rand.Next(Directions.Count)];
        }

        public void Respawn(GameStateModel session)
        {
            var rand = new Random();
            bool spawned = false;
            while(!spawned)
            {
                Point point = new Point(rand.Next(session.Grid.Width), rand.Next(session.Grid.Height));
                foreach(var enemy in session.Enemies)
                {
                    if(point == enemy.Position || session.Grid.GetTile(point.X, point.Y).Type == EnumTileType.Wall)
                    {
                        break;
                    }
                    Position = point;
                    spawned = true;
                    break;
                }
            }
        }
    }
}