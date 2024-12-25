using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderDeliveryReceiveOrderFunction.Models;

namespace OrderDeliveryReceiveOrderFunction;

public class OrderDeliveryReceiveOrderFunction
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    private readonly string _databaseName;
    private readonly string _containerName;

    public OrderDeliveryReceiveOrderFunction(CosmosClient cosmosClient, 
        ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _cosmosClient = cosmosClient;
        _logger = loggerFactory.CreateLogger<OrderDeliveryReceiveOrderFunction>();
        _configuration = configuration;

        _databaseName = "deliveryorderitems";
        _containerName = "Orders";
    }

    [Function("OrderDeliveryReceiveOrderFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("Processing order request.");

        // Read and deserialize the order data from the request body
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var order = JsonSerializer.Deserialize<Order>(requestBody);

        // Get the container reference
        var database = _cosmosClient.GetDatabase(_databaseName);
        var container = database.GetContainer(_containerName);

        // Insert the order data into CosmosDB
        await container.CreateItemAsync(order, new PartitionKey(order.Id));

        // Return success response
        return new OkObjectResult("Order request uploaded successfully.");
    }
}
