using DotnetCodeMaturityLab.Features.AI;
using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Tests.Features.AI;

public sealed class AiSuggestionContractTests
{
    [Theory]
    [InlineData("delivery", true)]
    [InlineData("product", true)]
    [InlineData("approve-discount", false)]
    public async Task ProviderFake_CannotBypassTheReviewBoundary(string category, bool expected)
    {
        IAiOrderSuggestionService fake = new FakeProvider(
            new(category, "Suggestion only", "Human decides", false));

        var suggestion = await fake.SuggestAsync("customer note", AppLanguage.English);

        Assert.Equal(expected, AiSuggestionContract.IsReviewable(suggestion));
    }

    [Fact]
    public void SimulatedTimeout_IsExplicitlyFallbackAndReviewable()
    {
        var fallback = AiProviderFailureScenario.TimeoutFallback(AppLanguage.English);

        Assert.True(fallback.IsFallback);
        Assert.True(AiSuggestionContract.IsReviewable(fallback));
        Assert.Contains("Simulated timeout", fallback.Reason);
    }

    private sealed class FakeProvider(AiOrderSuggestion result) : IAiOrderSuggestionService
    {
        public Task<AiOrderSuggestion> SuggestAsync(
            string customerNote, AppLanguage language,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(result);
        }
    }
}
