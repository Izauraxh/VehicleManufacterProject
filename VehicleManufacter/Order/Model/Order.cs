namespace OrderService.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public OrderStatus Status { get; set; }
        
    }
    public enum OrderStatus
    {
        Placed,
        Processing,
        Completed,
        Cancelled
    }
}
