using PacMan.Shared.Enums;
using PacMan.Shared.Factories;

namespace PacMan.Shared.Models
{
    public class TileGridBuilder
    {
        private int _width = 30;
        private int _height = 30;
        private Dictionary<string, Tile> Tiles { get; set; } = new();
        private TileFactory _pelletsFactory;
        private TileFactory _wallsFactory;

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
            _pelletsFactory = new PelletTileFactory();
            _wallsFactory = new WallTileFactory();
            Tiles = new();
            var rnd = new Random();
            for (var i = 0; i < tiles; i++)
            {
                var r1 = rnd.Next(0, _height);
                var r2 = rnd.Next(0, _width);
                var tile = GetTile(r1, r2);
                if (tile is { Type: EnumTileType.Wall })
                {
                    tile = _wallsFactory.CreateTile();
                    Tiles.Add($"{r1}_{r2}", tile);
                }
            }

            // intuitively it makes no sense to me why i is width and j is height but here we are
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    var tile = GetTile(i, j);
                    if (tile?.Type != EnumTileType.Wall)
                    {
                        Tiles.Add($"{i}_{j}", _pelletsFactory.CreateTile());
                    }
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
            return Tiles[$"{x}_{y}"];
        }

        public TileGrid Build()
        {
            return new(_width, _height, Tiles);
        }
    }
}