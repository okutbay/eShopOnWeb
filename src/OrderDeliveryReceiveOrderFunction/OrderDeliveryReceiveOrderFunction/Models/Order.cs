using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDeliveryReceiveOrderFunction.Models;

public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ShippingAddress { get; set; }
    public List<Item> Items { get; set; }
    public decimal FinalPrice { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public class Item
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}