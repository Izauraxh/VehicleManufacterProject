namespace OrderService
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime CreatedDate { get; set; }
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
