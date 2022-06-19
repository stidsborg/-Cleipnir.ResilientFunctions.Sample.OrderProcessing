using Microsoft.AspNetCore.Mvc;
using Orders.BusinessLogic;
using Orders.DataAccess;
using Orders.Domain;

namespace Orders.Controllers;

[ApiController]
[Route("[controller]")]
public class DeveloperController : ControllerBase
{
    private readonly IOrdersRepository _ordersRepository;
    
    public DeveloperController(IOrdersRepository ordersRepository) => _ordersRepository = ordersRepository;

    [HttpDelete]
    public async Task Delete() => await _ordersRepository.DeleteAllEntries();
}