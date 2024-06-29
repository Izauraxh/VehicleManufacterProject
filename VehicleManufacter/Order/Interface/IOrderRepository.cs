using OrderService.Model;

namespace OrderService.Interface
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        // Other methods
    }
}
