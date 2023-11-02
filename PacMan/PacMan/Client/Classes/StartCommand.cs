using Microsoft.AspNetCore.SignalR.Client;
using PacMan.Client.Services;
using PacMan.Shared.Models;

namespace PacMan.Client.Classes
{
    // i like the way this sucks
    public class StartCommand : IGameCommand<HubConnection?, TileGridBuilderOptions?>
    {
        public async Task<bool> Execute(HubConnection? connection, TileGridBuilderOptions? options)
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
    }
}