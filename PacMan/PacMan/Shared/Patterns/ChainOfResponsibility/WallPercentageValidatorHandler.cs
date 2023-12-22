using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.ChainOfResponsibility
{
    public record WallPercentageValidatorHandler : ITileGridValidatorHandler
    {
        private ITileGridValidatorHandler? _nextHandler;
        private const double MaxWallPercentage = 0.5;
        private const string Error = "Rule 2: Walls must not exceed 50% of the grid.";

        public void SetNext(ITileGridValidatorHandler handler)
        {
            _nextHandler = handler;
        }

        public string Validate(TileGrid grid)
        {
            var wallTileCount = (double)grid.Tiles.Count(tile => tile.Value.Type == EnumTileType.Wall);
            if (wallTileCount / (grid.Width * grid.Height) > MaxWallPercentage)
            {
                return _nextHandler?.Validate(grid) ?? string.Empty;
            }

            Console.WriteLine(Error);
            return Error;
        }
    }
}