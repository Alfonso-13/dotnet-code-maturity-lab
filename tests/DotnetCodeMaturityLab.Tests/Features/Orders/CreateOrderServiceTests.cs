using DotnetCodeMaturityLab.Features.Orders;

namespace DotnetCodeMaturityLab.Tests.Features.Orders;

public sealed class CreateOrderServiceTests
{
    [Fact]
    public async Task Create_WithValidData_CreatesOrderAndCalculatesSubtotal()
    {
        // Arrange
        var store = new InMemoryOrderStore();
        var service = new CreateOrderService(store);
        var request = new CreateOrderRequest("MONITOR-24", 2, 3499.90m);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Order);
        Assert.Equal(6999.80m, result.Order.Subtotal);
        Assert.Equal(result.Order, await store.FindAsync(result.Order.Id));
    }

    [Fact]
    public async Task Create_WithoutProductCode_ReturnsValidationError()
    {
        // Arrange
        var service = CreateService();
        var request = new CreateOrderRequest(" ", 2, 3499.90m);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(nameof(CreateOrderRequest.ProductCode), result.Errors.Keys);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_WithInvalidQuantity_ReturnsValidationError(int quantity)
    {
        // Arrange
        var service = CreateService();
        var request = new CreateOrderRequest("MONITOR-24", quantity, 3499.90m);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(nameof(CreateOrderRequest.Quantity), result.Errors.Keys);
    }

    [Fact]
    public async Task Create_WithInvalidUnitPrice_ReturnsValidationError()
    {
        // Arrange
        var service = CreateService();
        var request = new CreateOrderRequest("MONITOR-24", 2, 0m);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(nameof(CreateOrderRequest.UnitPrice), result.Errors.Keys);
    }

    private static CreateOrderService CreateService() =>
        new(new InMemoryOrderStore());
}
