using Orders.Communication;
using Orders.DataAccess;
using Orders.Domain;

namespace Orders.BusinessLogic;

public class OrderProcessor
{
    private readonly IBankClient _bankClient;
    private readonly IProductsClient _productsClient;
    private readonly IEmailClient _emailClient;
    private readonly ILogisticsClient _logisticsClient;
    private readonly IOrdersRepository _ordersRepository;

    public OrderProcessor(
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
    }

    public async Task CompleteOrder(Order order)
    {
        var productPrices = await _productsClient.GetProductPrices(order.ProductIds);
        var totalPrice = productPrices.Sum(p => p.Price);
        
        var bankTransactionId = Guid.NewGuid();
        await _bankClient.Reserve(bankTransactionId, totalPrice);
        await _logisticsClient.ShipProducts(order.ProductIds);
        await _bankClient.Capture(bankTransactionId);
        await _emailClient.SendOrderConfirmation(order.CustomerEmail, order.ProductIds);

        await _ordersRepository.Insert(order);
    }
}