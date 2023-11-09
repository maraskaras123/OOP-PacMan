namespace PacMan.Client.Services
{
    public interface IGameCommand<in T>
    {
        Task<bool> Execute(T parameter);
    }

    public interface IGameCommand<in T1, in T2>
    {
        Task<bool> Execute(T1 p1, T2 p2);
    }
}