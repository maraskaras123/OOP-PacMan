using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.ChainOfResponsibility
{
    public record PelletPercentageValidatorHandler : ITileGridValidatorHandler
    {
        private ITileGridValidatorHandler? _nextHandler;
        private const double MinPelletPercentage = 0.3;
        private const string Error = "Rule 3: There should be at least 30% of pellets.";

        public void SetNext(ITileGridValidatorHandler handler)
        {
            _nextHandler = handler;
        }

        public string Validate(TileGrid grid)
        {
            var pelletTileCount = (double)grid.Tiles.Count(tile => tile.Value.Type == EnumTileType.Pellet);
            if (pelletTileCount / (grid.Width * grid.Height) > MinPelletPercentage)
            {
                return _nextHandler?.Validate(grid) ?? string.Empty;
            }

            Console.WriteLine(Error);
            return Error;
        }
    }
}