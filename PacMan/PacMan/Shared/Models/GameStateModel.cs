using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class GameStateModel
    {
        public EnumDirection Direction { get; set; }
        public Point Coordinates { get; set; }
        public GameStateModel()
        {
            this.Direction = EnumDirection.Right;
            this.Coordinates = new Point(0, 0);
        }

        public GameStateModel(Point point)
        {
            this.Direction = EnumDirection.Right;
            this.Coordinates = point;
        }
    }
   
}