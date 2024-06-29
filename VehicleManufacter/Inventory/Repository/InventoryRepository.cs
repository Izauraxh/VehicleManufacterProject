using InventoryService.Data;
using InventoryService.Interfaces;
using InventoryService.Model;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;

        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Component> GetComponentByTypeAsync(string componentType)
        {
            return await _context.Components.FirstOrDefaultAsync(c => c.ComponentType == componentType);
        }

        public async Task UpdateComponentQuantityAsync(string componentType, int quantity)
        {
            var component = await _context.Components.FirstOrDefaultAsync(c => c.ComponentType == componentType);
            if (component != null)
            {
                component.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }
    }
}
