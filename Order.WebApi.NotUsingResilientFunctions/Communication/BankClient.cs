namespace Orders.Communication;

public interface IBankClient
{
    Task Reserve(Guid transactionId, decimal amount);
    Task Capture(Guid transactionId);
    Task CancelReservation(Guid transactionId);
}

public class BankClientStub : IBankClient
{
    public Task Reserve(Guid transactionId, decimal amount) => Task.CompletedTask;
    public Task Capture(Guid transactionId) => Task.CompletedTask;
    public Task CancelReservation(Guid transactionId) => Task.CompletedTask;
}