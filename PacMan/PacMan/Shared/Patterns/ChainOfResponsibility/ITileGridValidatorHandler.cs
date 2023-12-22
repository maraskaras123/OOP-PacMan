using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.ChainOfResponsibility
{
    public interface ITileGridValidatorHandler
    {
        void SetNext(ITileGridValidatorHandler handler);
        string Validate(TileGrid grid);
    }
}