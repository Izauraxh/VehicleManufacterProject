using MassTransit;
using MediatR;
using OrderService.Event;
using OrderService.Interface;
using OrderService.Model;

public class PlaceOrderCommand : IRequest<Unit>
{
    public OrderRequest OrderRequest { get; }

    public PlaceOrderCommand(OrderRequest orderRequest)
    {
        OrderRequest = orderRequest;
    }
}

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand,Unit>
{
    private readonly IOrderRepository _repository;

    private readonly IPublishEndpoint _publishEndpoint;

    public PlaceOrderCommandHandler(IOrderRepository repository, IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {



        // check inventory
        // 




        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.OrderRequest.CustomerId,
            Items = request.OrderRequest.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ComponentType = i.ComponentType,
                Quantity = i.Quantity
            }).ToList(),
            Status = OrderStatus.Placed
        };

        await _repository.AddOrderAsync(order);
        await _publishEndpoint.Publish(new OrderPlacedEvent(order));

        return Unit.Value;
    }
}
