using PacMan.Shared.Enums;
using System.Drawing;

namespace PacMan.Shared.Models
{
    public class PlayerStateModel : PlayerStateBaseModel
    {
        public EnumDirection Direction { get; set; }
        public Point Coordinates { get; set; }
        public int Points { get; set; }

        public PlayerStateModel()
        {
            Direction = EnumDirection.Right;
            Coordinates = new(0, 0);
        }

        public PlayerStateModel(Point point)
        {
            Direction = EnumDirection.Right;
            Coordinates = point;
        }

        public PlayerStateModel UpdateDirection(EnumDirection direction)
        {
            var model = (PlayerStateModel)MemberwiseClone();
            model.Direction = direction;
            return model;
        }
    }
}