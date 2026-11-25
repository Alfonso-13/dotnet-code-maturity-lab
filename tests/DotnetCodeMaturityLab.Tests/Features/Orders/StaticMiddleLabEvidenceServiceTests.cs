using DotnetCodeMaturityLab.Features.Orders;

namespace DotnetCodeMaturityLab.Tests.Features.Orders;

public sealed class StaticMiddleLabEvidenceServiceTests
{
    [Fact]
    public async Task PagesEvidence_ExecutesRulesInMemoryAndIdentifiesSimulation()
    {
        var store = new InMemoryOrderStore();
        var evidence = new StaticMiddleLabEvidenceService(new CreateOrderService(store), store);

        var result = await evidence.VerifyAsync();

        Assert.True(result.Passed);
        Assert.Contains("DEMO SIMULATION", result.Detail);
        Assert.Contains("No HTTP or SQLite", result.Detail);
        Assert.Equal(1, await store.CountAsync());
    }
}
