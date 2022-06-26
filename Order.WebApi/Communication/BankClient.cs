namespace Order.WebApi.Communication;

public interface IBankClient
{
    Task Reserve(decimal amount);
    Task Capture();
    Task CancelReservation();
}

public class BankClientStub : IBankClient
{
    public Task Reserve(decimal amount) 
        => Task.Delay(1_000).ContinueWith(_ => Console.WriteLine($"BANK: Reserved '{amount}'"));
    public Task Capture() 
        => Task.Delay(1_000).ContinueWith(_ => Console.WriteLine("BANK: Reserved amount captured"));
    public Task CancelReservation() 
        => Task.Delay(1_000).ContinueWith(_ => Console.WriteLine("BANK: Reservation cancelled"));
}