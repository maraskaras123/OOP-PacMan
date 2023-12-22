using PacMan.Shared.Enums;

namespace PacMan.Shared.Patterns.Flyweight
{
    public class TileRendererFactory
    {
        private readonly Dictionary<string, ITileRenderer> _renderers = new();

        public ITileRenderer GetRenderer(string key)
        {
            if (_renderers.TryGetValue(key, out var renderer))
            {
                return renderer;
            }

            if (Enum.TryParse<EnumTileType>(key, out var tileType))
            {
                renderer = new TileTypeRenderer(TilesHtml.TilesHtmlStrings[tileType]);
            }

            if (key.StartsWith("Enemy"))
            {
                var color = key.Split("_")[1];
                renderer = new TileTypeRenderer(
                    "<svg width=\"20px\" height=\"20px\" viewBox=\"0 0 24 24\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">" +
                    $"<path fill-rule=\"evenodd\" clip-rule=\"evenodd\" d=\"M22 19.2058V12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12V19.2058C2 20.4896 3.35098 21.3245 4.4992 20.7504C5.42726 20.2864 6.5328 20.3552 7.39614 20.9308C8.36736 21.5782 9.63264 21.5782 10.6039 20.9308L10.9565 20.6957C11.5884 20.2744 12.4116 20.2744 13.0435 20.6957L13.3961 20.9308C14.3674 21.5782 15.6326 21.5782 16.6039 20.9308C17.4672 20.3552 18.5727 20.2864 19.5008 20.7504C20.649 21.3245 22 20.4896 22 19.2058ZM16 10.5C16 11.3284 15.5523 12 15 12C14.4477 12 14 11.3284 14 10.5C14 9.67157 14.4477 9 15 9C15.5523 9 16 9.67157 16 10.5ZM9 12C9.55228 12 10 11.3284 10 10.5C10 9.67157 9.55228 9 9 9C8.44772 9 8 9.67157 8 10.5C8 11.3284 8.44772 12 9 12Z\" fill=\"#{color}\" />" +
                    "</svg>");
            }

            if (key.StartsWith("Player"))
            {
                var color = key.Split("_")[1];
                renderer = new PlayerTileRenderer(
                    $"<svg transform='{{0}}' height='18px' width='18px' version='1.1' id='Capa_1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 52 52' xml:space='preserve'><g><path style='fill:#{color};' d='M44.385,44.385c-10.154,10.154-26.616,10.154-36.77,0s-10.154-26.616,0-36.77s26.616-10.154,36.77,0L26,26L44.385,44.385z'/></g></svg>");
            }

            if (renderer is null)
            {
                throw new($"No renderer found for {key}");
            }

            _renderers.Add(key, renderer);

            return renderer;
        }
    }
}