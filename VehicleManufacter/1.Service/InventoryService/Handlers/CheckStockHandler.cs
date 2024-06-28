using InventoryService.Interfaces;
using MediatR;

namespace InventoryService.Handlers
{
    public class CheckStockQuery : IRequest<int>
    {
        public string ComponentType { get; }

        public CheckStockQuery(string componentType)
        {
            ComponentType = componentType;
        }
    }

    public class CheckStockQueryHandler : IRequestHandler<CheckStockQuery, int>
    {
        private readonly IInventoryRepository _repository;

        public CheckStockQueryHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CheckStockQuery request, CancellationToken cancellationToken)
        {
            var component = await _repository.GetComponentByTypeAsync(request.ComponentType);
            return component?.Quantity ?? 0;
        }
    }
}
