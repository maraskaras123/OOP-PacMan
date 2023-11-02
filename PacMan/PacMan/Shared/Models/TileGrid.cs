using System.Drawing;

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
            return Tiles.TryGetValue($"{x}_{y}", out var tile) ? tile : new();
        }

        public Tile GetTile(Point point)
        {
            return GetTile(point.X, point.Y);
        }
    }
}