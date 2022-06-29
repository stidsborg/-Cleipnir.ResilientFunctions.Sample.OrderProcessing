namespace Orders.WebApi.Communication;

public interface IBankClient
{
    Task Reserve(decimal amount);
    Task Capture();
    Task CancelReservation();
}

public class BankClientStub : IBankClient
{
    public Task Reserve(decimal amount) 
        => Task.Delay(Constants.ExternalServiceDelay).ContinueWith(_ => Console.WriteLine($"BANK: Reserved '{amount}'"));
    public Task Capture() 
        => Task.Delay(Constants.ExternalServiceDelay).ContinueWith(_ => Console.WriteLine("BANK: Reserved amount captured"));
    public Task CancelReservation() 
        => Task.Delay(Constants.ExternalServiceDelay).ContinueWith(_ => Console.WriteLine("BANK: Reservation cancelled"));
}