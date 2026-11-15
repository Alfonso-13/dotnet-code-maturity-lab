using System.Net;
using System.Net.Http.Json;
using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotnetCodeMaturityLab.Tests.Features.Orders;

public sealed class OrderEndpointsTests
{
    [Fact]
    public async Task PostOrder_ValidRequest_ReturnsCreatedAndPersistsOrder()
    {
        var (factory, databasePath) = CreateFactory();

        try
        {
            using var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync(
                "/api/orders",
                new CreateOrderRequest("MONITOR-24", 2, 3499.90m));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);

            var created = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
            Assert.NotNull(created);
            Assert.Equal(6999.80m, created.Subtotal);

            var persisted = await client.GetFromJsonAsync<CreateOrderResponse>(
                response.Headers.Location);
            Assert.NotNull(persisted);
            Assert.Equal(created.Id, persisted.Id);
            Assert.Equal("MONITOR-24", persisted.ProductCode);
        }
        finally
        {
            await factory.DisposeAsync();
            DeleteDatabase(databasePath);
        }
    }

    [Fact]
    public async Task PostOrder_InvalidRequest_ReturnsBadRequestWithoutPersisting()
    {
        var (factory, databasePath) = CreateFactory();

        try
        {
            using var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync(
                "/api/orders",
                new CreateOrderRequest("", 0, 0m));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var orders = await client.GetFromJsonAsync<CreateOrderResponse[]>("/api/orders");
            Assert.NotNull(orders);
            Assert.Empty(orders);
        }
        finally
        {
            await factory.DisposeAsync();
            DeleteDatabase(databasePath);
        }
    }

    [Fact]
    public async Task GetOrder_UnknownId_ReturnsNotFound()
    {
        var (factory, databasePath) = CreateFactory();

        try
        {
            using var client = factory.CreateClient();

            var response = await client.GetAsync($"/api/orders/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        finally
        {
            await factory.DisposeAsync();
            DeleteDatabase(databasePath);
        }
    }

    private static (WebApplicationFactory<Program> Factory, string DatabasePath) CreateFactory()
    {
        var databasePath = Path.Combine(
            Path.GetTempPath(),
            $"dotnet-maturity-http-{Guid.NewGuid():N}.db");
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IDbContextFactory<OrderDbContext>>();
                    services.RemoveAll<DbContextOptions<OrderDbContext>>();
                    services.AddDbContextFactory<OrderDbContext>(options =>
                        options.UseSqlite($"Data Source={databasePath};Pooling=False"));
                });
            });

        return (factory, databasePath);
    }

    private static void DeleteDatabase(string databasePath)
    {
        File.Delete(databasePath);
        File.Delete($"{databasePath}-shm");
        File.Delete($"{databasePath}-wal");
    }
}
