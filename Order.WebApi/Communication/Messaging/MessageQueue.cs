namespace Order.WebApi.Communication.Messaging;

public class MessageQueue
{
    public MessageQueue()
    {
        Subscribers += _ => { };
    }

    public event Action<object> Subscribers;
    public void Send(object msg)
    {
        Console.WriteLine("MESSAGE_QUEUE SENDING: " + msg.GetType());
        Task.Run(() => Subscribers.Invoke(msg));
    }
}