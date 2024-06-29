using Common;
using MassTransit;
using MediatR;
using OrderService.Event;
using OrderService.Interface;
using OrderService.Model;
using System.Net.Http;

public class PlaceOrderCommand : IRequest<Unit>
{
    public OrderRequest OrderRequest { get; }

    public PlaceOrderCommand(OrderRequest orderRequest)
    {
        OrderRequest = orderRequest;
    }
}

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Unit>
{
    private readonly IOrderRepository _repository;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPublishEndpoint _publishEndpoint;

    public PlaceOrderCommandHandler(IOrderRepository repository, LoggerFactoryWrapper loggerWrapper, IHttpClientFactory httpClientFactory, IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _loggerFactory = loggerWrapper.LoggerFactory;
        _httpClientFactory = httpClientFactory;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var logger = _loggerFactory.CreateLogger<LoggerFactoryWrapper>();

        logger.LogInformation(" Started Order Placement for customer" + request.OrderRequest.CustomerId);


        // check inventory 
        var client = _httpClientFactory.CreateClient("InventoryAPI");
        var hasStock = false;
        foreach (var item in request.OrderRequest.Items)
        {
            
            var response = await client.GetAsync($"checkstock/{item.Id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                hasStock = bool.Parse(content);
                if (hasStock) 
                logger.LogInformation($"Item " + item.ComponentType + " has stock");
                else
                 logger.LogInformation($"Item " + item.ComponentType + " doesn't have stock");
                //produce 
            }
        }



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
