using Orders.WebApi.Domain;

namespace Orders.WebApi.Communication;

public interface IProductsClient
{
    Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds);
}

public class ProductsClientStub : IProductsClient
{
    public Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds)
        => Task.Delay(Constants.ExternalServiceDelay).ContinueWith(_ =>
        {
            Console.WriteLine("PRODUCTS_SERVER: Product prices calculated");
            return productIds.Select(id => new ProductPrice(id, 100M));
        });
}