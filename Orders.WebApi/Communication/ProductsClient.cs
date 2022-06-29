using Order.WebApi.Domain;

namespace Order.WebApi.Communication;

public interface IProductsClient
{
    Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds);
}

public class ProductsClientStub : IProductsClient
{
    public Task<IEnumerable<ProductPrice>> GetProductPrices(IEnumerable<Guid> productIds)
        => Task.Delay(1_000).ContinueWith(_ =>
        {
            Console.WriteLine("PRODUCTS_SERVER: Product prices calculated");
            return productIds.Select(id => new ProductPrice(id, 100M));
        });
}