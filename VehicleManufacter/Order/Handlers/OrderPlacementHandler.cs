using Common;
using MassTransit;
using MediatR;
using OrderService.Event;
using OrderService.Interface;
using OrderService.Model;
using OrderService;
using System.Net.Http;
using System.Security.Cryptography;

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
            
            var response = await client.GetAsync($"CheckStock/{item.ComponentType}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                hasStock = int.Parse(content)>=item.Quantity;
                if (hasStock)
                    logger.LogInformation($"Item " + item.ComponentType + " has stock");
                else
                {
                    logger.LogInformation($"Item " + item.ComponentType + " doesn't have stock");
                    //produce 
                    break;
                }
            }
        }

        if (hasStock)
        {



            var order = new OrderService.Order
            {

                CustomerId = request.OrderRequest.CustomerId,
                OrderItems = request.OrderRequest.Items.Select(i => new OrderItem
                {
                    ComponentType = i.ComponentType,
                    Quantity = i.Quantity
                }).ToList(),
                Status = OrderStatus.Placed
            };

            await _repository.AddOrderAsync(order);
            logger.LogInformation($"Order placed");
       
        await _publishEndpoint.Publish(new OrderPlacedEvent(order));
        }
        return Unit.Value;
    }
}
