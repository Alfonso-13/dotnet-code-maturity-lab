using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.AspNetCore.Components;

namespace DotnetCodeMaturityLab.Tests.Features.Orders;

public sealed class ServerMiddleLabEvidenceServiceTests
{
    [Fact]
    public async Task NonLoopbackNavigationOrigin_IsRejectedBeforeAnyRequest()
    {
        var navigation = new TestNavigationManager("https://example.invalid/");

        var result = await new ServerMiddleLabEvidenceService(navigation).VerifyAsync();

        Assert.False(result.Passed);
        Assert.Contains("loopback", result.Detail, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager(string origin) => Initialize(origin, origin);

        protected override void NavigateToCore(string uri, bool forceLoad) => throw new NotSupportedException();
    }
}
