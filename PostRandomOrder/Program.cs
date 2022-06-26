using System.Text;

namespace PostRandomOrder;

internal static class Program
{
    private static int Main(string[] args)
    {
        int port;
        try
        {
            port = int.Parse(args[0]);
        }
        catch (Exception)
        {
            Console.WriteLine("Usage: PostRandomOrder port");
            return -1;
        }

        var httpClient = new HttpClient();
        var orderId = Guid.NewGuid().ToString();
        var body = @"
            {
                ""orderId"": ""@ORDER_ID"",
                ""customerId"": ""@CUSTOMER_ID"",
                ""productIds"": [ ""@PRODUCT_ID_1"", ""@PRODUCT_ID_2"" ]
            }"
            .Replace("@ORDER_ID", orderId)
            .Replace("@CUSTOMER_ID", Guid.NewGuid().ToString())
            .Replace("@PRODUCT_ID_1", Guid.NewGuid().ToString())
            .Replace("@PRODUCT_ID_2", Guid.NewGuid().ToString());

        Console.WriteLine($"Posting order id: '{orderId}'");
        var url = $"http://localhost:{port}/Orders";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        return 0;
    }
}