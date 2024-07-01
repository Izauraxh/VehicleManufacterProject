using Microsoft.EntityFrameworkCore;
using OrderService.Model;

namespace OrderService
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity relationships
            // Configure auto-increment for Order and OrderItem IDs
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Id)
                .ValueGeneratedOnAdd();
            // Configure relationship between Order and OrderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi=>oi.Order)
                                .HasForeignKey(oi => oi.OrderId);
            // Seed data
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CustomerId = "CU1" ,CreatedDate=DateTime.Now, Status=OrderStatus.Placed},
                new Order { Id = 2, CustomerId="CU2",CreatedDate=DateTime.Now ,Status=OrderStatus.Processing}
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ComponentType = "Engine", Quantity = 1, Price = 1000 },
                new OrderItem { Id = 2, OrderId = 1, ComponentType = "Chassis", Quantity = 1, Price = 5000 },
                new OrderItem { Id = 3, OrderId = 2, ComponentType = "Option Pack", Quantity = 1, Price = 1500 }
            );
        }
    }
}
