using Sample.WebApi.ExternalServices;
using Sample.WebApi.V4;

namespace Sample.WebApi.Ordering;

public static class IoCBindings
{
    public static void AddOrderProcessingBindings(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<OrderProcessor>();
        serviceCollection.AddScoped<OrderProcessor.Inner>();

        var messageBroker = new MessageBroker();
        var paymentProviderStub = new PaymentProviderStub(messageBroker);
        var logisticsServiceStub = new LogisticsServiceStub(messageBroker);
        var emailServiceStub = new EmailServiceStub(messageBroker);

        serviceCollection.AddSingleton(messageBroker);
    }
}