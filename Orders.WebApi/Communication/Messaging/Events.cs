namespace Order.WebApi.Communication.Messaging;

public record OrderConfirmationEmailSent(string RequestId, Guid CustomerId);