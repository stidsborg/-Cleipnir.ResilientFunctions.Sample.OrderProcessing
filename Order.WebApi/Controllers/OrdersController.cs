using Microsoft.AspNetCore.Mvc;
using Order.WebApi.BusinessLogic;

namespace Order.WebApi.Controllers;

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
            _logger.LogInformation("Started order processing: '{OrderId}'", order.OrderId);
            await _orderProcessor.CompleteOrder(order);
            _logger.LogInformation("Completed order processing: '{OrderId}'", order.OrderId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed order processing: '{OrderId}'", order.OrderId);
        }
    } 
}