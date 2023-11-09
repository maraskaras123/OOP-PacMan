using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class GameStateModel
    {
        public EnumDirection Direction { get; set; }
        public Point Coordinates { get; set; }
        public int Points { get; set; }

        public GameStateModel()
        {
            Direction = EnumDirection.Right;
            Coordinates = new(0, 0);
        }

        public GameStateModel(Point point)
        {
            Direction = EnumDirection.Right;
            Coordinates = point;
        }
    }
}