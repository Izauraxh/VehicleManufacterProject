using InventoryService.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{componentType}")]
        public async Task<IActionResult> CheckStock(string componentType)
        {
            var query = new CheckStockQuery(componentType);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
