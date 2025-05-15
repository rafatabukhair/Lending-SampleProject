using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace BusinessEntities
{
    public class Order : IdObject
    {
        public string Customer { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Total => OrderItems.Sum(i => i.Quantity * i.ItemPrice);
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal ItemPrice { get; set; }
    }
}