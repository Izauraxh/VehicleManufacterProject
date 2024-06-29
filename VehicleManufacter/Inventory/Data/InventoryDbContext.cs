using InventoryService.Model;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Component> Components { get; set; }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity relationships
        }
    }
}
