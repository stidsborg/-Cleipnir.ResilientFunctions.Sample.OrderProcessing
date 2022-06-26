using Order.WebApi.Communication;
using Order.WebApi.DataAccess;

namespace Order.WebApi.BusinessLogic;

public class OrderProcessor
{
    private readonly ILogger<OrderProcessor> _logger;
    private readonly IBankClient _bankClient;
    private readonly IProductsClient _productsClient;
    private readonly IEmailClient _emailClient;
    private readonly ILogisticsClient _logisticsClient;
    private readonly IOrdersRepository _ordersRepository;

    public OrderProcessor(
        ILogger<OrderProcessor> logger,
        IBankClient bankClient, 
        IProductsClient productsClient, 
        IEmailClient emailClient, 
        ILogisticsClient logisticsClient, 
        IOrdersRepository ordersRepository)
    {
        _logger = logger;
        _bankClient = bankClient;
        _productsClient = productsClient;
        _emailClient = emailClient;
        _logisticsClient = logisticsClient;
        _ordersRepository = ordersRepository;
    }

    public async Task CompleteOrder(Domain.Order order)
    {
        _logger.LogInformation($"Started {nameof(CompleteOrder)} invocation");
        var productPrices = await _productsClient.GetProductPrices(order.ProductIds);
        var totalPrice = productPrices.Sum(p => p.Price);
        
        _logger.LogInformation($"Processing order: {order.OrderId}");
        await Task.Delay(5_000);
        
        await _bankClient.Reserve(totalPrice);
        await _logisticsClient.ShipProducts(order.CustomerId, order.ProductIds);
        await _bankClient.Capture();
        await _emailClient.SendOrderConfirmation(order.CustomerId, order.ProductIds);

        await _ordersRepository.Insert(order);
        _logger.LogInformation($"Completed {nameof(CompleteOrder)} invocation");
    }
}