namespace PacMan.Shared.Patterns.Observer
{
    public class Publisher
    {
        public Dictionary<string, ISubscriber> Subscribers { get; } = new();

        public void AddSubscriber(string key, ISubscriber subscriber)
        {
            Subscribers.Add(key, subscriber);
        }

        public void RemoveSubscriber(string key)
        {
            Subscribers.Remove(key);
        }

        public async Task Notify(bool add, string name)
        {
            foreach (var subscriber in Subscribers) await subscriber.Value.Notify(add, name);
        }
    }
}