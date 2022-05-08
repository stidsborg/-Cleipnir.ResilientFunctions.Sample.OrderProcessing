using System.Text.Json;
using Dapper;
using Orders.Domain;

namespace Orders.DataAccess;

public interface IOrdersRepository
{
    Task<bool> Insert(Order order);
}

public class OrdersRepository : IOrdersRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;
    public OrdersRepository(SqlConnectionFactory sqlConnectionFactory) => _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<bool> Insert(Order order)
    {
        await using var conn = await _sqlConnectionFactory.Create();
        var affectedRows = await conn.ExecuteAsync(@"
            INSERT INTO orders 
                (order_id, products, customer_email)
            VALUES
                (@OrderId, @Products, @CustomerEmail)
            ON CONFLICT DO NOTHING;",
            new
            {
                OrderId = order.OrderId, 
                Products = JsonSerializer.Serialize(order.ProductIds), 
                CustomerEmail = order.CustomerEmail
            }
        );

        return affectedRows == 1;
    }
}