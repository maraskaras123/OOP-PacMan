namespace PacMan.Shared.Patterns.Observer
{
    public interface ISubscriber
    {
        Task Notify(bool add, string name);
    }
}