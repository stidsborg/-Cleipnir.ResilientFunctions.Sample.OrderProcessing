namespace Orders.Domain;

public record Order(string OrderId, string CustomerEmail, IEnumerable<string> ProductIds);