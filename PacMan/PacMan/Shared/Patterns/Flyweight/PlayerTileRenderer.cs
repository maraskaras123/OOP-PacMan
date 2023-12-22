using Microsoft.AspNetCore.Components;
using PacMan.Shared.Enums;

namespace PacMan.Shared.Patterns.Flyweight
{
    public class PlayerTileRenderer : ITileRenderer
    {
        private readonly string _tileHtml;

        public PlayerTileRenderer(string tileHtml)
        {
            _tileHtml = tileHtml;
        }

        public MarkupString Render(object? options)
        {
            if (Enum.TryParse<EnumDirection>(options?.ToString(), out var directionEnum))
            {
                var rotate = directionEnum switch
                {
                    EnumDirection.Up => "rotate(-90 0 0)",
                    EnumDirection.Down => "rotate(90 0 0)",
                    EnumDirection.Left => "rotate(180 0 0)",
                    _ => ""
                };
                return new(string.Format(_tileHtml, rotate));
            }

            return new(string.Format(_tileHtml, ""));
        }
    }
}