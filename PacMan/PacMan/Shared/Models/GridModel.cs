using System.Drawing;
using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class GridModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string, EnumTileType> Tiles { get; set; } = new();
         public GridModel()
        {
            Width = 30;
            Height = 30;
            Tiles = new Dictionary<string, EnumTileType>();
        }

        public GridModel(int width, int height, Dictionary<string, EnumTileType> tiles)
        {
            Width = width;
            Height = height;
            Tiles = tiles;
        }

        public EnumTileType GetTile(int x, int y)
        {
            if (Tiles.TryGetValue($"{x}_{y}", out EnumTileType tile))
            {
                return tile;
            }
            return EnumTileType.Empty;
        }
    }
}