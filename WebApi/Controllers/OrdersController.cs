using Microsoft.AspNetCore.Mvc;
using Orders.WebApi.BusinessLogic;

namespace Orders.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderProcessor _orderProcessor;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(OrderProcessor orderProcessor, ILogger<OrdersController> logger)
    {
        _orderProcessor = orderProcessor;
        _logger = logger;
    }

    [HttpPost]
    public async Task Post(Domain.Order order)
    {
        try
        {
            var orderId = order.OrderId;
            await _orderProcessor.CompleteOrder(orderId, order);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed order processing: '{OrderId}'", order.OrderId);
        }
    } 
}