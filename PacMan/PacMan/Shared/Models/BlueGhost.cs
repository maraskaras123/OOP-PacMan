using System.Drawing;

namespace PacMan.Shared.Models
{
    public class BlueGhost : IEnemy
    {
        public Point Position { get; set; }
        public char Character => 'B';

        private const int TicksPerMove = 2;

        private int _moveTicks = 1;

        public void Move(Dictionary<string, GameStateModel> playerStates)
        {
            _moveTicks--;
            if (_moveTicks == 0)
            {
                var nearestPlayer = FindNearestPlayer(playerStates);
                var path = AStar(Position, nearestPlayer.Coordinates);

                if (path is { Count: > 1 })
                {
                    Position = path[1]; // Move to the next step in the path
                }

                _moveTicks = TicksPerMove;
            }
        }

        private GameStateModel FindNearestPlayer(Dictionary<string, GameStateModel> playerStates)
        {
            var minDistance = double.MaxValue;
            GameStateModel? nearestPlayer = null;

            foreach (var playerState in playerStates.Values)
            {
                var distance = Math.Sqrt(Math.Pow(Position.X - playerState.Coordinates.X, 2) +
                                         Math.Pow(Position.Y - playerState.Coordinates.Y, 2));

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlayer = playerState;
                }
            }

            return nearestPlayer ?? throw new NullReferenceException();
        }

        private List<Point> AStar(Point start, Point end)
        {
            var storage = Storage.GetInstance();
            var openList = new List<Node>();
            var closedList = new List<Node>();

            openList.Add(new() { Position = start });

            while (openList.Any())
            {
                var currentNode = openList.OrderBy(node => node.F).First();

                if (currentNode.Position == end)
                {
                    var path = new List<Point>();
                    while (currentNode != null)
                    {
                        path.Add(currentNode.Position);
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();
                    return path.ToList();
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighborPos in GetNeighbors(currentNode.Position))
                {
                    // I'm assuming Storage.Walls is accessible from this scope
                    // Sorry i changed it up, maybe i should revert back to points
                    if (storage.Grid.GetTile(neighborPos.X, neighborPos.Y).Type == Enums.EnumTileType.Wall)
                    {
                        continue;
                    }

                    var neighbor = new Node { Position = neighborPos, Parent = currentNode };
                    neighbor.G = currentNode.G + 1;
                    neighbor.H = Math.Abs(neighbor.Position.X - end.X) + Math.Abs(neighbor.Position.Y - end.Y);

                    if (closedList.Any(node => node.Position == neighborPos) && currentNode.G + 1 >= neighbor.G)
                    {
                        continue;
                    }

                    if (!openList.Any(node => node.Position == neighborPos) || currentNode.G + 1 < neighbor.G)
                    {
                        openList.Add(neighbor);
                    }
                }
            }

            return null; // No path could be found
        }

        private IEnumerable<Point> GetNeighbors(Point current)
        {
            yield return new(current.X - 1, current.Y);
            yield return new(current.X + 1, current.Y);
            yield return new(current.X, current.Y - 1);
            yield return new(current.X, current.Y + 1);
        }

        private class Node
        {
            public Point Position { get; set; }
            public int G { get; set; }
            public int H { get; set; }
            public int F => G + H;
            public Node Parent { get; set; }
        }
    }
}