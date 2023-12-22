using PacMan.Shared.Enums;
using PacMan.Shared.Patterns.Visitor;

namespace PacMan.Shared.Models
{
    public abstract class Tile
    {
        public EnumTileType Type { get; set; }
        public abstract void AcceptVisitor(IVisitor visitor);
    }
}