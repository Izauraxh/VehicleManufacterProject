using System;
namespace OrderService
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string ComponentType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrderId {get;set;}
        public Order Order { get; set;}
        
    }
}