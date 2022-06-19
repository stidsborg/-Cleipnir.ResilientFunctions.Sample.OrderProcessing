using Order.WebApi.Domain;

namespace Order.WebApi.Communication;

public interface IProductsClient
{
    Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds);
}

public class ProductsClientStub : IProductsClient
{
    private readonly Dictionary<Guid, decimal> _prices = new();
    private readonly object _sync = new();
    private readonly Random _random = new();

    public Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds)
    {
        lock (_sync)
        {
            var toReturn = new List<ProductPrice>();
            foreach (var productId in productIds)
            {
                if (!_prices.ContainsKey(productId)) 
                    _prices[productId] = _random.Next(1, 100);    

                toReturn.Add(new ProductPrice(productId, _prices[productId]));
            }

            return Task.FromResult((IEnumerable<ProductPrice>) toReturn);
        }
    }
}