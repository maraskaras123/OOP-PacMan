using PacMan.Client.Services;
using System.Windows.Input;

namespace PacMan.Client.Classes
{
    public class Invoker
    {
        private IGameCommand? _command;

        public void SetCommand(IGameCommand command)
        {
            this._command = command;
        }

        public void InvokeCommand()
        {
            if (this._command is IGameCommand)
            {
                this._command.Execute();
            }
        }
    }
}
