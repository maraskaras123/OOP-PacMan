using PacMan.Shared.Enums;

namespace PacMan.Client.InputMethods
{
    public class ButtonsInputHandler : IInputHandler
    {
        public override void ChangeDirection(string key)
        {
            Console.WriteLine(key);
            EnumDirection? direction = key switch
            {
                "Up" => EnumDirection.Up,
                "Right" => EnumDirection.Right,
                "Down" => EnumDirection.Down,
                "Left" => EnumDirection.Left,
                _ => null
            };

            if (direction.HasValue)
            {
               HandleInputAsync(direction.Value);
            }
        }
    }
}