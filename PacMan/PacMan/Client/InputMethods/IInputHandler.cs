using Microsoft.AspNetCore.SignalR.Client;
using PacMan.Shared.Enums;

namespace PacMan.Client.InputMethods
{
    public abstract class IInputHandler
    {
        public HubConnection _hubConnection;
        public abstract void ChangeDirection(string key);
        public void SetupConnection(HubConnection connection)
        {
            _hubConnection = connection;
        }
         public async Task HandleInputAsync(EnumDirection direction)
        {
            if (_hubConnection is not null)
            {
                try
                {
                    await _hubConnection.InvokeAsync("OnChangeDirection", direction);
                    Console.WriteLine(direction);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " (ChangeDirection())");
                }
            }
            else
            {
                Console.WriteLine("Connection to hub is not set");
            }
        }
    }
}