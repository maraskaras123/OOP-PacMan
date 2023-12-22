using PacMan.Shared.Factories;
using PacMan.Shared.Models;

namespace PacMan.Shared.Patterns.Proxy
{
    public class TileFactoryProxy<T> : TileFactory where T : TileFactory, new()
    {
        private T? _factory;

        public override Tile CreateTile()
        {
            _factory ??= new();

            return _factory.CreateTile();
        }

        public override Tile ConvertToEmpty()
        {
            _factory ??= new();

            return _factory.ConvertToEmpty();
        }

        public override Tile ConvertToMegaPellet()
        {
            _factory ??= new();

            return _factory.ConvertToMegaPellet();
        }
    }
}