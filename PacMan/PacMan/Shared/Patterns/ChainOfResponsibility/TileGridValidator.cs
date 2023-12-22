using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.ChainOfResponsibility
{
    public record TileGridValidator
    {
        private readonly ITileGridValidatorHandler _handler;

        public TileGridValidator()
        {
            _handler = new EdgeValidatorHandler();
            var wallPercentageHandler = new WallPercentageValidatorHandler();
            var pelletPercentageHandler = new PelletPercentageValidatorHandler();
            var megaPelletPercentageHandler = new MegaPelletPercentageValidatorHandler();

            _handler.SetNext(wallPercentageHandler);
            wallPercentageHandler.SetNext(pelletPercentageHandler);
            pelletPercentageHandler.SetNext(megaPelletPercentageHandler);
        }

        public string Validate(TileGrid grid)
        {
            return _handler.Validate(grid);
        }
    }
}