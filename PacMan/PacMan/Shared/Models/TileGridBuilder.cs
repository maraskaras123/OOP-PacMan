using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class TileGridBuilder
    {
        private int Width = 30;
        private int Height = 30;
        private Dictionary<string, Tile> Tiles { get; set; } = new();

        public TileGridBuilder WithWidth(int width)
        {
            this.Width = width;
            return this;
        }

        public TileGridBuilder WithHeight(int height)
        {
            this.Height = height;
            return this;
        }

        public TileGridBuilder WithRandomTiles(int tiles = 50)
        {
            this.Tiles = new Dictionary<string, Tile>();
            var rnd = new Random();
            // intuitively it makes no sense to me why i is width and j is height but here we are
            for (int i = 0; i < Width; i++) 
            {
                for (int j = 0; j < Height; j++)
                {
                    this.Tiles.Add($"{i}_{j}", new Tile(EnumTileType.Pellet));
                }
            }
            for (var i = 0; i < tiles; i++)
            {
                int r1 = rnd.Next(0, this.Height);
                int r2 = rnd.Next(0, this.Width);
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
            this.Tiles = new Dictionary<string, Tile>();
            // some algorhithm
            return this;
        }

        private Tile GetTile(int x, int y)
        {
            if (Tiles.TryGetValue($"{x}_{y}", out Tile tile))
            {
                return tile;
            }
            return new Tile();
        }

        public TileGrid Build()
        {
            return new TileGrid(Width, Height, Tiles);
        }
    }
}
