namespace PacMan.Shared.Models
{
    public class TileGrid
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string, Tile> Tiles { get; set; }

        public TileGrid()
        {
            Width = 30;
            Height = 30;
            Tiles = new();
        }

        public TileGrid(int width, int height, Dictionary<string, Tile> tiles)
        {
            Width = width;
            Height = height;
            Tiles = tiles;
        }

        public Tile GetTile(int x, int y)
        {
            return Tiles.TryGetValue($"{x}_{y}", out var tile) ? tile : new EmptyTile();
        }

        public void ChangeTile(Tile tile, int x, int y)
        {
            var key = $"{x}_{y}";
            if (Tiles.ContainsKey(key))
            {
                Tiles[key] = tile;
            }
        }

        public GridModel ConvertForSending()
        {
            var grid = new GridModel
            {
                Width = Width,
                Height = Height
            };
            foreach (var tile in Tiles)
            {
                grid.Tiles.Add(tile.Key, tile.Value.Type);
            }

            return grid;
        }

        public TileGrid ShallowCopy()
        {
            return (TileGrid)MemberwiseClone();
        }

        public TileGrid DeepCopy()
        {
            var clone = ShallowCopy();
            clone.Tiles = new(Tiles);

            return clone;
        }
    }
}