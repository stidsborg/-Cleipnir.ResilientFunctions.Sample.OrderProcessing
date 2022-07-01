namespace Orders.WebApi.Communication.Messaging;

public class MessageQueue
{
    private readonly List<Func<object, Task>> _subscribers = new();
    private readonly object _lock = new();

    public void Subscribe(Func<object, Task> handler)
    {
        lock (_lock)
            _subscribers.Add(handler);
    }
    
    public void Send(object msg)
    {
        Console.WriteLine("MESSAGE_QUEUE SENDING: " + msg.GetType());
        Task.Run(async () =>
        {
            List<Func<object, Task>> subscribers;
            lock (_lock)
                subscribers = _subscribers.ToList();

            foreach (var subscriber in subscribers)
                await subscriber(msg);
        });
    }
}