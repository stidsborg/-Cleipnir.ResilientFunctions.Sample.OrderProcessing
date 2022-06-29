namespace Orders.WebApi.Communication.Messaging;

public record SendOrderConfirmationEmail(string RequestId, Guid CustomerId);