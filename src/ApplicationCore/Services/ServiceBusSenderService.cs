using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class ServiceBusSenderService : IServiceBusSenderService
{
    private readonly ServiceBusClient _client;
    private readonly string _queueName;

    public ServiceBusSenderService(string connectionString, string queueName)
    {
        _client = new ServiceBusClient(connectionString);
        _queueName = queueName;
    }

    public async Task SendMessageAsync(Order order)
    {
        var sender = _client.CreateSender(_queueName);
        var message = new ServiceBusMessage(JsonSerializer.Serialize(order));
        await sender.SendMessageAsync(message);
    }
}
