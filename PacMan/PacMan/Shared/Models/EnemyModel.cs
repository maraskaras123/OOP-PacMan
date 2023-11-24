using System.Drawing;

namespace PacMan.Shared.Models
{
    public class EnemyModel
    {
        public PointDto Position { get; set; }
        public char Character { get; set; }
    }

    public class PointDto
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}