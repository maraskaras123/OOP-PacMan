using System.Drawing;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

public abstract class GhostBase : IEnemy
{
    public Point Position { get; set; }

    public char Character { get; }
    public int TicksPerMove { get; set; }
    protected int _moveTicks = 1;


    protected GhostBase(char character, int ticksPerMove)
    {
        Character = character;
        TicksPerMove = ticksPerMove;
    }

    public void Move(GameStateModel session)
    {
        _moveTicks--;
        if (_moveTicks == 0)
        {
            var nearestPlayer = FindPlayer(session.State);
            Position = MovePattern(session, Position, nearestPlayer);
            _moveTicks = TicksPerMove;
        }
    }
    protected abstract Point FindPlayer(Dictionary<string, PlayerStateModel> playerStates);
    protected abstract Point MovePattern(GameStateModel session, Point start, Point end);
    public void Respawn(GameStateModel session)
    {
        var rand = new Random();
        bool spawned = false;
        while (!spawned)
        {
            Point point = new Point(rand.Next(session.Grid.Width), rand.Next(session.Grid.Height));
            foreach (var enemy in session.Enemies)
            {
                if (point == enemy.Position || session.Grid.GetTile(point.X, point.Y).Type == EnumTileType.Wall)
                {
                    break;
                }
                Position = point;
                spawned = true;
                break;
            }
        }
    }
    protected bool CanMoveTo(GameStateModel session, Point nextPosition)
    {

        foreach (var enemy in session.Enemies)
        {
            if (nextPosition == enemy.Position)
            {
                return false;
            }
        }

        // Check if the next position is a wall
        return session.Grid.GetTile(nextPosition.X, nextPosition.Y).Type != EnumTileType.Wall;
    }
}