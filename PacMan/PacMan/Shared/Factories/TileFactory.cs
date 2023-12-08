using PacMan.Shared.Models;

namespace PacMan.Shared.Factories
{
    public abstract class TileFactory
    {
        public abstract Tile CreateTile();

        public virtual Tile ConvertToEmpty()
        {
            return new EmptyTile();
        }
        public virtual Tile ConvertToMegaPellet()
        {
            return new MegaPelletTile();
        }
    }
}