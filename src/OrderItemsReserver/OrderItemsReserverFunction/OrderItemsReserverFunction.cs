using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OrderItemsReserverFunction;

public class OrderItemsReserverFunction
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<OrderItemsReserverFunction> _logger;
    private readonly IConfiguration _configuration;

    public OrderItemsReserverFunction(BlobServiceClient blobServiceClient, ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _logger = loggerFactory.CreateLogger<OrderItemsReserverFunction>();
        _configuration = configuration;
    }

    [Function("OrderItemsReserverFunction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
        ILogger log)
    {
        _logger.LogInformation("Processing order request.");

        // Read the order details from the request body
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var containerName = _configuration["OrderBlobContainerName"] ?? "orders";

        int maxRetries = 3;

        for (var retry = 1; retry <= maxRetries; retry++)
        {
            try
            {
                // Get reference to the Blob container and blob
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobName = $"{System.Guid.NewGuid()}.json";
                var blobClient = containerClient.GetBlobClient(blobName);

                // Upload the JSON data to blob storage
                using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));
                await blobClient.UploadAsync(memoryStream);

                // Return success response
                return new OkObjectResult("Order request uploaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to upload order on attempt {retry}: {ex.Message}.");
            }
        }

        return new ObjectResult(new ProblemDetails
        {
            Status = 500,
            Title = "Internal Server Error",
            Detail = $"Failed to upload order after '{maxRetries}' attempt(s)."
        })
        {
            StatusCode = 500
        };
    }
}
