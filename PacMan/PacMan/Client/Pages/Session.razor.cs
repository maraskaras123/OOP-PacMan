﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PacMan.Client.Classes;
using PacMan.Client.Services;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;

namespace PacMan.Client.Pages
{
    public partial class Session : IAsyncDisposable
    {
        private InputService Input { get; set; }
        private List<PlayerDto> PlayerInfo { get; set; } = new();
        private EnumGameState? GameState { get; set; }
        private GridModel Grid { get; set; } = new();
        private List<string> PlayerCoordinates { get; set; } = new();
        private List<EnemyModel> Enemies { get; set; } = new();

        public HubConnection? HubConnection { get; set; }
        public EnumInputMethod SelectedInputMethod { get; set; } = EnumInputMethod.Keyboard;
        public TileGridBuilderOptions GridOptions { get; } = new();
        public List<PlayerStateBaseModel> Players { get; set; } = new();
        public int EndPoints { get; set; } = 100;
        public string PlayerName { get; set; }
        public string CurrentPlayerName { get; set; }

        [Parameter]
        public string SessionId { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        #region ComponentMethods

        protected override async Task OnInitializedAsync()
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/gamehub"))
                .Build();
            HubConnection.On<List<PlayerStateBaseModel>>("Joined", OnJoined);
            HubConnection.On("JoinRejected", () => Navigation.NavigateTo("/session"));
            HubConnection.On<int, string>("PlayerUpdate", OnPlayerUpdate);
            HubConnection.On<EnumGameState>("StateChange", OnStateChange);
            HubConnection.On<GridModel>("ReceiveGrid", OnReceiveGrid);
            HubConnection.On<List<EnemyModel>>("ReceiveEnemies", OnReceiveEnemies);
            HubConnection.On<StateModel>("Tick", OnTick);
            PlayerName = $"Player {new Random().Next()}";
            CurrentPlayerName = PlayerName;
            SetInputMethod();

            try
            {
                await HubConnection.StartAsync();
                await HubConnection.InvokeAsync("JoinSession", SessionId, PlayerName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region HubListeners

        private Task OnJoined(List<PlayerStateBaseModel> players)
        {
            Players = players;
            StateHasChanged();
            return Task.CompletedTask;
        }

        private Task OnPlayerUpdate(int index, string name)
        {
            if (index > Players.Count)
            {
                Players.Add(new() { Name = name });
            }
            else
            {
                Players[index].Name = name;
            }

            if (name == PlayerName)
            {
                CurrentPlayerName = name;
            }

            StateHasChanged();

            return Task.CompletedTask;
        }

        private Task OnStateChange(EnumGameState state)
        {
            GameState = state;
            SetInputMethod();
            StateHasChanged();
            return Task.CompletedTask;
        }

        private Task OnReceiveGrid(GridModel receivedGrid)
        {
            Grid = receivedGrid;
            StateHasChanged();
            return Task.CompletedTask;
        }

        private Task OnReceiveEnemies(List<EnemyModel> receivedEnemies)
        {
            Enemies = receivedEnemies;
            StateHasChanged();
            return Task.CompletedTask;
        }

        private Task OnTick(StateModel model)
        {
            GameState = model.GameState;

            PlayerCoordinates = model.Coordinates;
            PlayerInfo = new();
            for (var i = 0; i < model.Points.Count; i++)
            {
                PlayerInfo.Add(new(i, Players[i].Name, model.Points[i]));
            }

            PlayerInfo = PlayerInfo.OrderByDescending(ps => ps.Points).ToList();
            StateHasChanged();
            return Task.CompletedTask;
        }

        #endregion

        #region ClickMethods

        public void OnChangeInputMethod(EnumInputMethod method)
        {
            SelectedInputMethod = method;
        }

        public void OnStart()
        {
            if (HubConnection is not null)
            {
                var invoker = new Invoker();
                invoker.SetCommand(new StartCommand(HubConnection, GridOptions, EndPoints));
                invoker.InvokeCommand();
            }

            StateHasChanged();
        }

        private async Task Restart()
        {
            if (HubConnection is not null)
            {
                if (HubConnection.State == HubConnectionState.Disconnected)
                {
                    await HubConnection.StartAsync();
                }

                await HubConnection.InvokeAsync("OnRestart");
            }
        }

        public async Task OnChangeName()
        {
            if (HubConnection is not null)
            {
                await HubConnection.InvokeAsync("OnChangeName", PlayerName);
            }
        }

        #endregion

        private void SetInputMethod()
        {
            Input = new(SelectedInputMethod, HubConnection);
        }

        private void OnLeave()
        {
            Navigation.NavigateTo("/session");
        }

        #region Functions

        // these really should just be svg files not these dumb ass functions
        private static string GetPacmanSvgString(string color = "EB343A", string rotate = "")
        {
            return
                $"<svg transform='{rotate}' height='18px' width='18px' version='1.1' id='Capa_1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 52 52' xml:space='preserve'><g><path style='fill:#{color};' d='M44.385,44.385c-10.154,10.154-26.616,10.154-36.77,0s-10.154-26.616,0-36.77s26.616-10.154,36.77,0L26,26L44.385,44.385z'/></g></svg>";
        }

        private static string GetGhostSvgString(string color = "2803ff")
        {
            return
                "<svg width=\"20px\" height=\"20px\" viewBox=\"0 0 24 24\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">" +
                $"<path fill-rule=\"evenodd\" clip-rule=\"evenodd\" d=\"M22 19.2058V12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12V19.2058C2 20.4896 3.35098 21.3245 4.4992 20.7504C5.42726 20.2864 6.5328 20.3552 7.39614 20.9308C8.36736 21.5782 9.63264 21.5782 10.6039 20.9308L10.9565 20.6957C11.5884 20.2744 12.4116 20.2744 13.0435 20.6957L13.3961 20.9308C14.3674 21.5782 15.6326 21.5782 16.6039 20.9308C17.4672 20.3552 18.5727 20.2864 19.5008 20.7504C20.649 21.3245 22 20.4896 22 19.2058ZM16 10.5C16 11.3284 15.5523 12 15 12C14.4477 12 14 11.3284 14 10.5C14 9.67157 14.4477 9 15 9C15.5523 9 16 9.67157 16 10.5ZM9 12C9.55228 12 10 11.3284 10 10.5C10 9.67157 9.55228 9 9 9C8.44772 9 8 9.67157 8 10.5C8 11.3284 8.44772 12 9 12Z\" fill=\"#{color}\" />" +
                "</svg>";
        }

        private static string GetPacmanColorByPlayer(int playerIndex, int direction = 1)
        {
            var directionEnum = (EnumDirection) direction;
            var rotate = directionEnum switch
            {
                EnumDirection.Up => "rotate(-90 0 0)",
                EnumDirection.Down => "rotate(90 0 0)",
                EnumDirection.Left => "rotate(180 0 0)",
                _ => ""
            };
            return playerIndex switch
            {
                0 => GetPacmanSvgString("F0C419", rotate),
                1 => GetPacmanSvgString("34EB5E", rotate),
                2 => GetPacmanSvgString("344FEB", rotate),
                3 => GetPacmanSvgString("EB34E5", rotate),
                _ => GetPacmanSvgString(rotate: rotate)
            };
        }

        private static string GetGhostColorByChar(char character)
        {
            return character switch
            {
                'B' => GetGhostSvgString("2803ff"),
                'R' => GetGhostSvgString("ff0303"),
                _ => GetGhostSvgString()
            };
        }

        private string GetPlayerScoreClass(PlayerDto player)
        {
            return player.PlayerName == CurrentPlayerName ? "text-primary" : "text-secondary";
        }

        #endregion
    }

    internal record PlayerDto
    {
        public string PlayerName { get; }
        public int PlayerId { get; }
        public int Points { get; }

        public PlayerDto(int playerId, string playerName, int points)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            Points = points;
        }
    }
}