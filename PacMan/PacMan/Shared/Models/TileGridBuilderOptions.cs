using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Shared.Models
{
    // this might be stupid
    // i could just build the grid on the client and send it over.
    // idk that could maybe theoretically possibly hypothetically cause security problems
    public class TileGridBuilderOptions 
    {
        public int Width {  get; set; }
        public int Height { get; set; }
        public string TileAlgorhithm { get; set; } // might make this an Enum later
        public int RandomTileCount { get; set; } 

        public TileGridBuilderOptions()
        {
            this.Width = 30;
            this.Height = 30;
            this.RandomTileCount = 50;
            this.TileAlgorhithm = "Random";
        }

        public TileGridBuilderOptions(int width, int height, string tileAlgorhithm)
        {
            this.Width = width;
            this.Height = height;
            this.TileAlgorhithm = tileAlgorhithm;
        }
    }
}
