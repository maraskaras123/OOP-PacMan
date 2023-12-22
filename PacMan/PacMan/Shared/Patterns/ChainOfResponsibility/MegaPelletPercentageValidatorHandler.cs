using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.ChainOfResponsibility
{
    public record MegaPelletPercentageValidatorHandler : ITileGridValidatorHandler
    {
        private ITileGridValidatorHandler? _nextHandler;
        private const double MinMegaPelletPercentage = 0.1;
        private const string Error = "Rule 4: There should be at least 10% of mega pellets.";

        public void SetNext(ITileGridValidatorHandler handler)
        {
            _nextHandler = handler;
        }

        public string Validate(TileGrid grid)
        {
            var megaPelletCount = (double)grid.Tiles.Count(tile => tile.Value.Type == EnumTileType.MegaPellet);
            if (megaPelletCount / (grid.Width * grid.Height) > MinMegaPelletPercentage)
            {
                return _nextHandler?.Validate(grid) ?? string.Empty;
            }

            Console.WriteLine(Error);
            return Error;
        }
    }
}