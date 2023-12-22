namespace PacMan.Shared.Models
{
    // this might be stupid
    // i could just build the grid on the client and send it over.
    // idk that could maybe theoretically possibly hypothetically cause security problems
    public class TileGridBuilderOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string TileAlgorithm { get; set; } // might make this an Enum later
        public int RandomTileCount { get; set; }
        public int? SelectedGridId { get; set; }

        public TileGridBuilderOptions()
        {
            Width = 30;
            Height = 30;
            RandomTileCount = 50;
            TileAlgorithm = "Random";
        }

        public TileGridBuilderOptions(int width, int height, string tileAlgorithm)
        {
            Width = width;
            Height = height;
            TileAlgorithm = tileAlgorithm;
        }
    }
}