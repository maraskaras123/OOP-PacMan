using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PacMan.Client.Classes;
using PacMan.Client.InputMethods;
using PacMan.Client.Services;

namespace PacMan.Client
{
    public class Program
    {
        public static readonly string ApiUrl = "https://pacman-api.arnasm.dev";

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress =
                    new(builder.HostEnvironment.IsDevelopment() ? builder.HostEnvironment.BaseAddress : ApiUrl)
            });
            builder.Services.AddSingleton<IInputHandler, KeyboardInputHandler>();
            builder.Services.AddSingleton<InputService>();
            builder.Services.AddScoped<StartCommand>();

            await builder.Build().RunAsync();
        }
    }
}