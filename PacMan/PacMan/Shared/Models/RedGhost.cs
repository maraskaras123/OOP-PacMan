using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PacMan.Shared.Models
{
    public class RedGhost : IEnemy
    {
        public char Character => 'R';
        public Point Position { get; set; }
        private int ticksSinceLastDirectionChange = 0;
        private Point currentDirection = Directions[0]; // Default to right

        private static readonly List<Point> Directions = new List<Point>
        {
            new Point(1, 0),  // Right
            new Point(-1, 0), // Left
            new Point(0, 1),  // Down
            new Point(0, -1)  // Up
        };

        public void Move(Dictionary<string, GameStateModel> playerStates)
        {
            ticksSinceLastDirectionChange++;
            var storage = Storage.GetInstance();

            if (ticksSinceLastDirectionChange >= 4)
            {
                currentDirection = ChooseRandomDirection();
                ticksSinceLastDirectionChange = 0;
            }

            var nextPosition = new Point(Position.X + currentDirection.X, Position.Y + currentDirection.Y);

            // Assuming Storage.Walls is accessible from this scope
            // Sorry i changed it up, maybe i should revert back to points
            if (storage.Grid.GetTile(nextPosition.X, nextPosition.Y).Type != Enums.EnumTileType.Wall&&(CanMoveTo(nextPosition)))
            {
                Position = nextPosition;
            }
            else
            {
                currentDirection = ChooseRandomDirection();
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
            if (storage.Grid.GetTile(nextPosition.X, nextPosition.Y).Type == Enums.EnumTileType.Wall)
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