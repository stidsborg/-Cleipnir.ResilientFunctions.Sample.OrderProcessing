using Microsoft.AspNetCore.Mvc;
using Order.WebApi.DataAccess;

namespace Order.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DeveloperController : ControllerBase
{
    private readonly IOrdersRepository _ordersRepository;
    
    public DeveloperController(IOrdersRepository ordersRepository) => _ordersRepository = ordersRepository;

    [HttpDelete]
    public async Task Delete() => await _ordersRepository.DeleteAllEntries();
}