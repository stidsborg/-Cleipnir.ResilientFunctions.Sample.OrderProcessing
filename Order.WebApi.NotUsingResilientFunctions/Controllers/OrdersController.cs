using Microsoft.AspNetCore.Mvc;
using Orders.BusinessLogic;
using Orders.Domain;

namespace Orders.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderProcessor _orderProcessor;

    public OrdersController(OrderProcessor orderProcessor) => _orderProcessor = orderProcessor;

    [HttpPost]
    public async Task Post(Order order)
    {
        await _orderProcessor.CompleteOrder(order);
    } 
}