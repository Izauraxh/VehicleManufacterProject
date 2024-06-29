using System;
namespace OrderService.Model
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public string ComponentType { get; set; }
        public int Quantity { get; set; }
    }
}