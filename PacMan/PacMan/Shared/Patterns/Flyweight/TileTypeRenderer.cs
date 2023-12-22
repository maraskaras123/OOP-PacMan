using Microsoft.AspNetCore.Components;

namespace PacMan.Shared.Patterns.Flyweight
{
    public record TileTypeRenderer : ITileRenderer
    {
        private readonly string _tileHtml;

        public TileTypeRenderer(string tileHtml)
        {
            _tileHtml = tileHtml;
        }

        public MarkupString Render(object? options = null)
        {
            return new(_tileHtml);
        }
    }
}