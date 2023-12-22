using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public abstract class Tile
    {
        public EnumTileType Type { get; set; }
        public abstract void AcceptVisitor(IVisitor visitor);
    }

    public class DtoTile : Tile
    {
        public override void AcceptVisitor(IVisitor visitor)
        {
            visitor.VisitEmptyTile();
        }
    }

    public class TileGridDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string, DtoTile> Tiles { get; set; }

        public static implicit operator TileGrid(TileGridDto dto)
        {
            var grid = new TileGrid
            {
                Width = dto.Width,
                Height = dto.Height,
                Tiles = new()
            };
            foreach (var tile in dto.Tiles)
            {
                grid.Tiles.Add(tile.Key, tile.Value);
            }

            return grid;
        }
    }
}