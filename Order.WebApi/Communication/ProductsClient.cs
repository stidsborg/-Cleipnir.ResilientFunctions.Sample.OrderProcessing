using Order.WebApi.Domain;

namespace Order.WebApi.Communication;

public interface IProductsClient
{
    Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds);
}

public class ProductsClientStub : IProductsClient
{
    public Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds) 
        => Task.FromResult(productIds.Select(id => new ProductPrice(id, 100M)));
}