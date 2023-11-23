namespace PacMan.Shared.Observer
{
    public interface ISubscriber
    {
        Task Notify(bool add, string name);
    }
}