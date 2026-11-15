using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.EntityFrameworkCore;

namespace DotnetCodeMaturityLab.Tests.Features.Orders;

public sealed class SqliteOrderStoreTests
{
    [Fact]
    public async Task AddAsync_ReopeningContext_PreservesOrder()
    {
        // Arrange
        var databasePath = Path.Combine(
            Path.GetTempPath(),
            $"dotnet-maturity-lab-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseSqlite($"Data Source={databasePath};Pooling=False")
            .Options;
        var factory = new TestContextFactory(options);

        try
        {
            await using (var database = await factory.CreateDbContextAsync())
            {
                await database.Database.EnsureCreatedAsync();
            }

            var store = new SqliteOrderStore(factory);
            var service = new CreateOrderService(store);

            // Act
            var created = await service.CreateAsync(
                new CreateOrderRequest("MONITOR-24", 2, 3499.90m));
            var persisted = await store.FindAsync(created.Order!.Id);

            // Assert
            Assert.True(created.IsSuccess);
            Assert.NotNull(persisted);
            Assert.Equal("MONITOR-24", persisted.ProductCode);
            Assert.Equal(6999.80m, persisted.Subtotal);
            Assert.Equal(1, await store.CountAsync());
        }
        finally
        {
            File.Delete(databasePath);
        }
    }

    private sealed class TestContextFactory(DbContextOptions<OrderDbContext> options)
        : IDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext() => new(options);

        public Task<OrderDbContext> CreateDbContextAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult(CreateDbContext());
    }
}
