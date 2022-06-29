namespace Orders.WebApi.Domain;

public record Order(string OrderId, Guid CustomerId, IEnumerable<Guid> ProductIds);