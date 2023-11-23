namespace PacMan.Shared.Observer
{
    public class Subscriber : ISubscriber
    {
        public Func<bool, string, Task> NotifyFunc { get; init; }

        public Subscriber(Func<bool, string, Task> notifyFunc)
        {
            NotifyFunc = notifyFunc;
        }

        public async Task Notify(bool add, string name)
        {
            await NotifyFunc.Invoke(add, name);
        }
    }
}