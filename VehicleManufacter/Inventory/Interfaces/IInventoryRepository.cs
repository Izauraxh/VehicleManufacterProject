using InventoryService.Model;

namespace InventoryService.Interfaces
{
    public interface IInventoryRepository
    {
        Task<Component> GetComponentByTypeAsync(string componentType);
        Task UpdateComponentQuantityAsync(string componentType, int quantity);
    }
}
