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
            modelBuilder.Entity<Component>()
             .Property(c => c.Id)
             .ValueGeneratedOnAdd(); // Configure Id as auto-incremented identity

            modelBuilder.Entity<Component>()
                .HasIndex(c => c.ComponentType)
                .IsUnique(); // Ensure ComponentType is unique

            // Seed initial data if needed
            modelBuilder.Entity<Component>().HasData(
                new Component { Id = 1, ComponentType = "Engine", Quantity = 10 },
                new Component { Id = 2, ComponentType = "Chassis", Quantity = 5 },
                new Component { Id = 3, ComponentType = "Option Pack", Quantity = 20 }
            );

            base.OnModelCreating(modelBuilder);


        }
    }
}
