using Microsoft.AspNetCore.SignalR.Client;
using PacMan.Client.InputMethods;
using PacMan.Shared.Enums;

namespace PacMan.Client.Services
{
    public class InputService
    {
        private IInputHandler _currentInputHandler;

        public InputService(EnumInputMethod inputMethod, HubConnection connection)
        {
            switch (inputMethod)
            {
                case EnumInputMethod.Keyboard:
                    _currentInputHandler = new KeyboardInputHandler();
                    break;
                case EnumInputMethod.Buttons:
                    _currentInputHandler = new ButtonsInputHandler();
                    break;
            }
            if(_currentInputHandler != null)
            {
                _currentInputHandler.SetupConnection(connection);
            }
        }

        public void ChangeDirection(string key)
        {
            _currentInputHandler.ChangeDirection(key);
        }
    }
}
