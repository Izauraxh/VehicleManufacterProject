using OrderService.Model;

namespace OrderService.Event
{
    public class OrderPlacedEvent
    {
        public Order Order { get; }

        public OrderPlacedEvent(Order order)
        {
            Order = order;
        }
    }
}
