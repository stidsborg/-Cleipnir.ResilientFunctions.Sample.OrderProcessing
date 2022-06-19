using Dapper;
using Order.WebApi.BusinessLogic;
using Order.WebApi.Communication;
using Order.WebApi.DataAccess;

namespace Order.WebApi;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        const string connectionString = "Server=localhost;Port=5432;Userid=postgres;Password=Pa55word!;Database=postgres;";
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

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }

    private static async Task InitializeTable(string connectionString)
    {
        await using var conn = await SqlConnectionFactory.Create(connectionString);
        await conn.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS orders (
                order_id UUID PRIMARY KEY,
                products TEXT,
                customer_id UUID
            );"
        );
    }
}