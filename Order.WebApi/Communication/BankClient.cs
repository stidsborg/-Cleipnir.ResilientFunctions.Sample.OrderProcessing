namespace Order.WebApi.Communication;

public interface IBankClient
{
    Task Reserve(decimal amount);
    Task Capture();
    Task CancelReservation();
}

public class BankClientStub : IBankClient
{
    public Task Reserve(decimal amount) => Task.CompletedTask;
    public Task Capture() => Task.CompletedTask;
    public Task CancelReservation() => Task.CompletedTask;
}