﻿namespace Orders.Communication;

public interface ILogisticsClient
{
    Task ShipProducts(IEnumerable<string> productIds);
}

public class LogisticsClientStub : ILogisticsClient
{
    public Task ShipProducts(IEnumerable<string> productIds) => Task.CompletedTask;
}