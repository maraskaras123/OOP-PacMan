using Microsoft.AspNetCore.SignalR.Client;
using PacMan.Client.Services;
using PacMan.Shared.Models;

namespace PacMan.Client.Classes
{
    public class StartCommand : IGameCommand
    {
        private HubConnection _connection;
        private TileGridBuilderOptions _options;

        public StartCommand(HubConnection connection, TileGridBuilderOptions options)
        {
            _connection = connection;
            _options = options;
        }

        public async Task<bool> StartGame(HubConnection connection, TileGridBuilderOptions options)
        {
            if (connection is null || options is null)
            {
                return false;
            }

            try
            {
                if (connection.State == HubConnectionState.Disconnected)
                {
                    await connection.StartAsync();
                }

                await connection.InvokeAsync("OnStart", options);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Execute()
        {
            StartGame(_connection, _options);
        }
    }
}