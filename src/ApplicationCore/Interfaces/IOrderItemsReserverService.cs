﻿using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IOrderItemsReserverService
{
    Task<bool> ReserveOrderAsync(Order orderDetails);
}
