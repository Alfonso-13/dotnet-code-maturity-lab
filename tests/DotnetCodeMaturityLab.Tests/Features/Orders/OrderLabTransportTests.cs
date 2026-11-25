using DotnetCodeMaturityLab.Features.Localization;
using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.AspNetCore.Components;

namespace DotnetCodeMaturityLab.Tests.Features.Orders;

public sealed class OrderLabTransportTests
{
    [Fact]
    public async Task PagesTransport_UsesMemoryAndReturnsExplicitlySimulatedStatusShape()
    {
        var store = new InMemoryOrderStore();
        var transport = new StaticOrderLabTransport(new CreateOrderService(store));

        var created = await transport.SubmitAsync(new("MONITOR-24", 2, 12.5m), AppLanguage.English);
        var rejected = await transport.SubmitAsync(new("", 0, 0m), AppLanguage.English);

        Assert.Equal(201, created.StatusCode);
        Assert.Equal(25m, created.Created?.Subtotal);
        Assert.Equal(400, rejected.StatusCode);
        Assert.Equal(1, await store.CountAsync());
    }

    [Fact]
    public async Task ServerTransport_RejectsNonLoopbackOriginBeforeSending()
    {
        var transport = new ServerOrderLabTransport(new TestNavigationManager("https://example.invalid/"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            transport.SubmitAsync(new("MONITOR-24", 1, 1m), AppLanguage.English));
    }

    private sealed class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager(string origin) => Initialize(origin, origin);

        protected override void NavigateToCore(string uri, bool forceLoad) => throw new NotSupportedException();
    }
}
