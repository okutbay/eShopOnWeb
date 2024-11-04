using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Services;

public class OrderItemsReserverService : IOrderItemsReserverService
{
    private readonly HttpClient _httpClient;

    public OrderItemsReserverService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ReserveOrderAsync(Order orderDetails)
    {
        var response = await _httpClient.PostAsJsonAsync("OrderItemsReserverFunction", orderDetails);
        response.EnsureSuccessStatusCode();
    }
}
