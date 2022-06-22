namespace Order.WebApi.Communication.Messaging;

public class EmailServiceStub 
{
    private readonly MessageQueue _messageQueue;

    public EmailServiceStub(MessageQueue messageQueue)
    {
        _messageQueue = messageQueue;
        messageQueue.Subscribers += MessageHandler;
    }

    private void MessageHandler(object message)
    {
        if (message is not SendOrderConfirmationEmail command)
            return;

        Task.Run(async () =>
        {
            await Task.Delay(1_000);
            _messageQueue.Send(new OrderConfirmationEmailSent(command.RequestId, command.CustomerId));
        });
    }
}