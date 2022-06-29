namespace Orders.WebApi.Communication.Messaging;

public record OrderConfirmationEmailSent(string RequestId, Guid CustomerId);