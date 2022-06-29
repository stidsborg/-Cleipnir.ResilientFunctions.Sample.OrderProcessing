using Dapper;
using Orders.WebApi.BusinessLogic;
using Orders.WebApi.Communication;
using Orders.WebApi.Communication.Messaging;
using Orders.WebApi.DataAccess;

namespace Orders.WebApi;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var port = args.Any() ? int.Parse(args[0]) : 5000;
        
        const string connectionString = "Server=localhost;Port=5432;Userid=postgres;Password=Pa55word!;Database=presentation;";
        await InitializeTable(connectionString);
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));
        builder.Services.AddSingleton<OrderProcessor>();
        builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();
        
        builder.Services.AddSingleton<IBankClient, BankClientStub>();
        builder.Services.AddSingleton<IProductsClient, ProductsClientStub>();
        builder.Services.AddSingleton<IEmailClient, EmailClientStub>();
        builder.Services.AddSingleton<ILogisticsClient, LogisticsClientStub>();

        var messageQueue = CreateAndSetupMessageQueue();
        builder.Services.AddSingleton(messageQueue);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API");
            options.RoutePrefix = string.Empty;
        });

        app.MapControllers();

        await app.RunAsync($"http://localhost:{port}");
    }

    private static async Task InitializeTable(string connectionString)
    {
        await using var conn = await SqlConnectionFactory.Create(connectionString);
        await conn.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS orders (
                order_id VARCHAR(255) PRIMARY KEY,
                products TEXT,
                customer_id UUID
            );"
        );
    }

    private static MessageQueue CreateAndSetupMessageQueue()
    {
        var messageQueue = new MessageQueue();
        _ = new EmailServiceStub(messageQueue);
        return messageQueue;
    }
}