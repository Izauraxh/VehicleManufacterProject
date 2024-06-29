using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
    {
        var command = new PlaceOrderCommand(request);
        await _mediator.Send(command);
        return Ok();
    }
}