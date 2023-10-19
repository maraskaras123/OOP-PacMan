using PacMan.Shared.Enums;

namespace PacMan.Shared.Models
{
    public class Tile
    {
        public EnumTileType Type { get; set; }

        public Tile()
        {
            this.Type = EnumTileType.Empty;
        }

        public Tile(EnumTileType type)
        {
            this.Type = type;
        }

        public void SetToEmpty()
        {
            this.Type = EnumTileType.Empty;
        }
    }
}
