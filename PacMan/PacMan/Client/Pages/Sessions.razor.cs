using Microsoft.AspNetCore.Components;
using PacMan.Shared.Enums;
using PacMan.Shared.Models;
using System.Net.Http.Json;

namespace PacMan.Client.Pages
{
    public partial class Sessions
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        private List<SessionStateBaseModel> SessionList { get; set; } = new();

        private bool IsLoaded { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshSessions();
            IsLoaded = true;
        }

        private void OnJoin(string key)
        {
            Navigation.NavigateTo($"/session/{key}");
        }

        private async Task OnCreate()
        {
            var key = await (await HttpClient.PostAsync("/sessions", null)).Content.ReadAsStringAsync();
            Navigation.NavigateTo($"/session/{key}");
        }

        private async Task RefreshSessions()
        {
            await Task.Delay(1000);
            SessionList = await HttpClient.GetFromJsonAsync<List<SessionStateBaseModel>>("/sessions") ?? new();
        }

        private string GetStateClass(EnumGameState state)
        {
            return state switch
            {
                EnumGameState.Initializing => "badge-success",
                EnumGameState.Finished => "badge-danger",
                _ => "badge-warning"
            };
        }
    }
}
