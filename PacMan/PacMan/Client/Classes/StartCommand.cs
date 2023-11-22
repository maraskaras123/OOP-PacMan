using Microsoft.AspNetCore.SignalR.Client;
using PacMan.Client.Services;
using PacMan.Shared.Models;

namespace PacMan.Client.Classes
{
    public class StartCommand : IGameCommand
    {
        private readonly HubConnection _connection;
        private readonly TileGridBuilderOptions _options;
        private readonly int _endPoints;

        public StartCommand(HubConnection connection, TileGridBuilderOptions options, int endPoints)
        {
            _connection = connection;
            _options = options;
            _endPoints = endPoints;
        }

        public async Task<bool> StartGame(HubConnection connection, TileGridBuilderOptions options, int endPoints)
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

                await connection.InvokeAsync("OnStart", options, endPoints);
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
            StartGame(_connection, _options, _endPoints);
        }
    }
}