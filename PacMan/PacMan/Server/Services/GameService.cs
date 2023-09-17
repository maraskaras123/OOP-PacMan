using PacMan.Shared;
using PacMan.Shared.Enums;

namespace PacMan.Server.Services
{
    public interface IGameService
    {
        void Reset();
        void AddPlayer(string userId);
        void RemovePlayer(string userId);
        Task Start();
        void Finish();
    }

    public class GameService : IGameService
    {
        public void Reset()
        {
            Storage.State = EnumGameState.Initializing;
            Storage.UserIds = new ();
        }

        public void AddPlayer(string userId)
        {
            Storage.UserIds.Add(userId);
        }

        public void RemovePlayer(string userId)
        {
            Storage.UserIds.Remove(userId);
        }

        public async Task Start()
        {
            Storage.State = EnumGameState.Starting;
            await Task.Delay(3000);
            Storage.State = EnumGameState.Running;
        }

        public void Finish()
        {
            Storage.State = EnumGameState.Finished;
        }
    }
}