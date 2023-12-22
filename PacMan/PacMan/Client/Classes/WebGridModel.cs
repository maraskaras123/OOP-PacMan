using System.Drawing;
using PacMan.Shared.Patterns.Flyweight;

namespace PacMan.Client.Classes
{
    public class WebGridModel
    {
        public GridTileType GridTileType { get; set; }
        public Point Coordinate { get; set; }
        public ITileRenderer Renderer { get; set; }
        public object? Options { get; set; }
    }

    public enum GridTileType
    {
        Player,
        Enemy,
        Other
    }
}