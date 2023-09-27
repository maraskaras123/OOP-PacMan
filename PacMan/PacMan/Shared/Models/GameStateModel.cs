using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class GameStateModel
    {
        public EnumDirection Direction { get; set; }
        public Point Coordinates { get; set; }
    }
}