using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class TileGridBuilder
    {
        private int _width = 30;
        private int _height = 30;
        private Dictionary<string, Tile> Tiles { get; set; } = new();

        public TileGridBuilder WithWidth(int width)
        {
            _width = width;
            return this;
        }

        public TileGridBuilder WithHeight(int height)
        {
            _height = height;
            return this;
        }

        public TileGridBuilder WithRandomTiles(int tiles = 50)
        {
            Tiles = new();
            var rnd = new Random();
            // intuitively it makes no sense to me why i is width and j is height but here we are
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    Tiles.Add($"{i}_{j}", new(EnumTileType.Pellet));
                }
            }

            for (var i = 0; i < tiles; i++)
            {
                var r1 = rnd.Next(0, _height);
                var r2 = rnd.Next(0, _width);
                var tile = GetTile(r1, r2);
                if (tile != null)
                {
                    tile.Type = EnumTileType.Wall;
                }
            }

            return this;
        }

        public TileGridBuilder WithClassicPacmanTiles()
        {
            Tiles = new();
            // some algorhithm
            return this;
        }

        private Tile? GetTile(int x, int y)
        {
            return Tiles.TryGetValue($"{x}_{y}", out var tile) ? tile : null;
        }

        public TileGrid Build()
        {
            return new(_width, _height, Tiles);
        }
    }
}