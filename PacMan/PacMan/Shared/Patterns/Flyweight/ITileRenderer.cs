using Microsoft.AspNetCore.Components;

namespace PacMan.Shared.Patterns.Flyweight
{
    public interface ITileRenderer
    {
        MarkupString Render(object? options = null);
    }
}