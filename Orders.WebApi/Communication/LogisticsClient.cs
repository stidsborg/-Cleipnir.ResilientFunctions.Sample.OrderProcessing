namespace Orders.WebApi.Communication;

public interface ILogisticsClient
{
    Task ShipProducts(Guid customerId, IEnumerable<Guid> productIds);
}

public class LogisticsClientStub : ILogisticsClient
{
    public Task ShipProducts(Guid customerId, IEnumerable<Guid> productIds)
        => Task.Delay(1_000).ContinueWith(_ => Console.WriteLine("LOGISTICS_SERVER: Products shipped"));
}