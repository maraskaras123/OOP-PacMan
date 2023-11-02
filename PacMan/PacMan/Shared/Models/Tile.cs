using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class Tile
    {
        public EnumTileType Type { get; set; }

        public Tile()
        {
            Type = EnumTileType.Empty;
        }

        public Tile(EnumTileType type)
        {
            Type = type;
        }

        public void SetToEmpty()
        {
            Type = EnumTileType.Empty;
        }
    }
}