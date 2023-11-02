namespace PacMan.Client.Services
{
    public interface IGameCommand<T>
    {
        void Execute(T parameter);
    }
}