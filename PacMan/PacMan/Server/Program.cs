using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using PacMan.Server.DbSchema;
using PacMan.Server.Hubs;
using PacMan.Server.Services;
using PacMan.Shared.Converters;

namespace PacMan.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jsonPath = "appsettings.json";
            if (builder.Environment.EnvironmentName == "Production")
            {
                jsonPath = "/config/appsettings-pacmanapi.json";
            }

            Console.WriteLine(jsonPath);
            builder.Configuration.AddJsonFile(jsonPath, false, true);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddControllers();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddSignalR(o => { o.EnableDetailedErrors = true; });
            builder.Services
                .AddSignalR(o => { o.EnableDetailedErrors = true; })
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.Converters.Add(new TileJsonConverter());
                });
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
            });

            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin",
                    options => options.AllowAnyOrigin() /*.WithOrigins(builder.Configuration["AllowedHosts"])*/
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseResponseCompression();
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            //app.MapRazorPages();
            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");
            app.MapHub<GameHub>("/gamehub");

            app.MapFallbackToFile("index.html");
            app.Run();
        }
    }
}