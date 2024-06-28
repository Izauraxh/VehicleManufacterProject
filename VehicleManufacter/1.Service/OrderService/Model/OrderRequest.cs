namespace OrderService.Model
{
    public class OrderRequest
    {
        public string CustomerId { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    }

    public class OrderItemRequest
    {
        public string ComponentType { get; set; }
        public int Quantity { get; set; }
    }
}
