using Cleipnir.ResilientFunctions;
using Cleipnir.ResilientFunctions.AspNetCore;
using Orders.WebApi.Communication;
using Orders.WebApi.DataAccess;
using Orders.WebApi.Domain;

namespace Orders.WebApi.BusinessLogic;

public class OrderProcessor : IRegisterRFuncOnInstantiation
{
    private readonly IBankClient _bankClient;
    private readonly IProductsClient _productsClient;
    private readonly IEmailClient _emailClient;
    private readonly ILogisticsClient _logisticsClient;
    private readonly IOrdersRepository _ordersRepository;

    public OrderProcessor(
        RFunctions rFunctions, 
        IBankClient bankClient, 
        IProductsClient productsClient, 
        IEmailClient emailClient, 
        ILogisticsClient logisticsClient, 
        IOrdersRepository ordersRepository)
    {
        _bankClient = bankClient;
        _productsClient = productsClient;
        _emailClient = emailClient;
        _logisticsClient = logisticsClient;
        _ordersRepository = ordersRepository;
        
        CompleteOrder = rFunctions.RegisterAction<Order>(
            functionTypeId: nameof(OrderProcessor),
            inner: _CompleteOrder
        ).Invoke;
    }

    public readonly RAction.Invoke<Order> CompleteOrder;
    private async Task _CompleteOrder(Order order)
    {
        Console.WriteLine($"ORDER_PROCESSOR: Started processing order: '{order.OrderId}'");
        var productPrices = await _productsClient.GetProductPrices(order.ProductIds);
        var totalPrice = productPrices.Sum(p => p.Price);

        await _bankClient.Reserve(totalPrice);
        await _logisticsClient.ShipProducts(order.CustomerId, order.ProductIds);
        await _bankClient.Capture();
        await _emailClient.SendOrderConfirmation(order.CustomerId, order.ProductIds);

        await _ordersRepository.Insert(order);
        Console.WriteLine($"ORDER_PROCESSOR: Completed processing order: '{order.OrderId}'");
    }
}