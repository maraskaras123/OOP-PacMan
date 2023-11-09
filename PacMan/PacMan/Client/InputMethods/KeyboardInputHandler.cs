using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PacMan.Shared.Enums;

namespace PacMan.Client.InputMethods
{
    public class KeyboardInputHandler : IInputHandler
    {
        private static KeyboardInputHandler _currentInstance;

        public KeyboardInputHandler()
        {
            _currentInstance = this;
        }
        public override async void ChangeDirection(string key)
        {
            EnumDirection? direction = key switch
            {
                "ArrowUp" => EnumDirection.Up,
                "ArrowRight" => EnumDirection.Right,
                "ArrowDown" => EnumDirection.Down,
                "ArrowLeft" => EnumDirection.Left,
                _ => null
            };

            if (direction.HasValue)
            {
               await HandleInputAsync(direction.Value);
            }
        }
        [JSInvokable]
        public static async Task HandleKeyboardInput(string key)
        {
            _currentInstance.ChangeDirection(key);
        }

    }
}