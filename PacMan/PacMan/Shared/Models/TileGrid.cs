using PacMan.Shared.Enums;

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
            if (Tiles.TryGetValue($"{x}_{y}", out Tile tile))
            {
                return tile;
            }
            return new EmptyTile();
        }
        public void ChangeTile(Tile tile, int x, int y)
        {
            string key = $"{x}_{y}";
            if(Tiles.ContainsKey(key))
            {
                Tiles[key] = tile;
            }
        }
        public GridModel ConvertForSending()
        {
            GridModel grid = new GridModel();
            grid.Width = Width;
            grid.Height = Height;
            foreach(var tile in Tiles)
            {
                grid.Tiles.Add(tile.Key, tile.Value.Type);
            }
            return grid;
        }
    }
}