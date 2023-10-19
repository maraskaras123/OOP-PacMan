namespace PacMan.Shared.Models
{
    public class TileGrid
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string, Tile> Tiles { get; set; } = new();

        public TileGrid()
        {
            Width = 30;
            Height = 30;
            Tiles = new Dictionary<string, Tile>();
        }

        public TileGrid(int width, int height, Dictionary<string, Tile> tiles)
        {
            Width = width;
            Height = height;
            Tiles = tiles;
        }

        public Tile GetTile(int x, int y)
        {
            if (Tiles.TryGetValue($"{x}_{y}", out Tile tile))
            {
                return tile;
            }
            return new Tile();
        }
    }
}
