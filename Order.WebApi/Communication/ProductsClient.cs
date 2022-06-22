using Cleipnir.ResilientFunctions.Helpers;
using Order.WebApi.Domain;

namespace Order.WebApi.Communication;

public interface IProductsClient
{
    Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds);
}

public class ProductsClientStub : IProductsClient
{
    public Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds) 
        => productIds.Select(id => new ProductPrice(id, 100M)).ToTask();
}