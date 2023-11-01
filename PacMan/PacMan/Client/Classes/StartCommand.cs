using Microsoft.AspNetCore.SignalR.Client;
using PacMan.Client.Services;
using PacMan.Shared.Models;

namespace PacMan.Client.Classes
{
    // i like the way this sucks
    public class StartCommand : IGameCommand<(HubConnection, TileGridBuilderOptions)>
    {
        public async void Execute((HubConnection, TileGridBuilderOptions) options)
        {
            if (options.Item1 is not null)
            {
                try
                {
                    if (options.Item1.State == HubConnectionState.Disconnected)
                    {
                        await options.Item1.StartAsync();
                    }
                    await options.Item1.InvokeAsync("OnStart", options.Item2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
