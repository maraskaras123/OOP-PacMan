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
            
            // increase width and height to account for outer border of walls
            _width += 2;
            _height += 2;

            Tiles = new();
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (j == 0 || i == 0 || i == _width - 1 || j == _height - 1 )
                    {
                        var tile = _wallsFactory.CreateTile();
                        Tiles.Add($"{i}_{j}", tile);
                    }
                }
            }


            var rnd = new Random();
            for (var i = 0; i < tiles; i++)
            {
                var r1 = rnd.Next(0, _width);
                var r2 = rnd.Next(0, _height);
                var tile = GetTile(r1, r2);
                if (tile is not { Type: EnumTileType.Wall })
                {
                    tile = _wallsFactory.CreateTile();
                    Tiles.Add($"{r1}_{r2}", tile);
                }
                
            }

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
            return Tiles.TryGetValue($"{x}_{y}", out var tile) ? tile : null;
        }

        public TileGrid Build()
        {
            return new(_width, _height, Tiles);
        }
    }
}