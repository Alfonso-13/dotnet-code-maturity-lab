using Microsoft.EntityFrameworkCore;

namespace DotnetCodeMaturityLab.Features.Orders;

public sealed class OrderDbContext(DbContextOptions<OrderDbContext> options)
    : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();

        order.ToTable("Orders");
        order.HasKey(entity => entity.Id);
        order.Property(entity => entity.ProductCode).HasMaxLength(80).IsRequired();
        order.Property(entity => entity.UnitPrice).HasPrecision(18, 2);
        order.Property(entity => entity.Subtotal).HasPrecision(18, 2);
        order.Property(entity => entity.Status).HasConversion<string>().HasMaxLength(24);
    }
}
