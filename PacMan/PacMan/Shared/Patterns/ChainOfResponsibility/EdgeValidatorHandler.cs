using PacMan.Shared.Enums;
using PacMan.Shared.Models;
using System.Drawing;

namespace PacMan.Shared.Patterns.ChainOfResponsibility
{
    public record EdgeValidatorHandler : ITileGridValidatorHandler
    {
        private ITileGridValidatorHandler? _nextHandler;
        private const string Error = "Rule 1: Edges of the grid must be walls.";

        public void SetNext(ITileGridValidatorHandler handler)
        {
            _nextHandler = handler;
        }

        public string Validate(TileGrid grid)
        {
            var edgeTiles = from tile in grid.Tiles
                let coordinates = tile.Key.Split("_")
                let point = new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1]))
                where point.X == 0 || point.X == grid.Width - 1 || point.Y == 0 || point.Y == grid.Height - 1
                select tile;
            if (edgeTiles.All(tile => tile.Value.Type == EnumTileType.Wall))
            {
                return _nextHandler?.Validate(grid) ?? string.Empty;
            }

            Console.WriteLine(Error);
            return Error;
        }
    }
}