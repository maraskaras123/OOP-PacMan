using System.Drawing;
using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class BlueGhost : GhostBase
    {
        public BlueGhost() : base( 'B', 2)
        {}

        protected override Point FindPlayer(Dictionary<string, PlayerStateModel> playerStates) //finds neaest player
        {
            var minDistance = double.MaxValue;
            PlayerStateModel? nearestPlayer = null;

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
            if(nearestPlayer != null)
            {
                return nearestPlayer.Coordinates;
            }
            return Point.Empty;
        }

        protected override Point MovePattern(GameStateModel session, Point start, Point end)
        {
            
            var openList = new List<Node>();
            var closedList = new List<Node>();

            openList.Add(new() { Position = start });

            while (openList.Any())
            {
                Node? currentNode = openList.OrderBy(node => node.F).First();

                if (currentNode.Position == end)
                {
                    var path = new List<Point>();
                    while (currentNode is not null)
                    {
                        path.Add(currentNode.Position);
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();
                    return path.ToList()[1];
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighborPos in GetNeighbors(currentNode.Position))
                {
                    // I'm assuming Storage.Walls is accessible from this scope
                    // Sorry i changed it up, maybe i should revert back to points
                    Console.WriteLine(CanMoveTo(session, neighborPos));
                    if (!CanMoveTo(session, neighborPos))
                    {
                        continue;
                    }

                    var neighbor = new Node
                    {
                        Position = neighborPos,
                        Parent = currentNode,
                        G = currentNode.G + 1
                    };
                    neighbor.H = Math.Abs(neighbor.Position.X - end.X) + Math.Abs(neighbor.Position.Y - end.Y);

                    if (closedList.Any(node => node.Position == neighborPos) && currentNode.G + 1 >= neighbor.G)
                    {
                        continue;
                    }

                    if (openList.All(node => node.Position != neighborPos) || currentNode.G + 1 < neighbor.G)
                    {
                        openList.Add(neighbor);
                    }
                }
            }
            return Position;
        }

        private static IEnumerable<Point> GetNeighbors(Point current)
        {
            yield return new(current.X - 1, current.Y);
            yield return new(current.X + 1, current.Y);
            yield return new(current.X, current.Y - 1);
            yield return new(current.X, current.Y + 1);
        }


        private class Node
        {
            public Point Position { get; init; }
            public int G { get; init; }
            public int H { get; set; }
            public int F => G + H;
            public Node? Parent { get; init; }
        }
    }
}