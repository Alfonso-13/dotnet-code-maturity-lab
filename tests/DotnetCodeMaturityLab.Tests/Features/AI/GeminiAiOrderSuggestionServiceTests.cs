using System.Net;
using System.Text;
using DotnetCodeMaturityLab.Features.AI;
using DotnetCodeMaturityLab.Features.Localization;
using Microsoft.Extensions.Configuration;

namespace DotnetCodeMaturityLab.Tests.Features.AI;

public sealed class GeminiAiOrderSuggestionServiceTests
{
    [Fact]
    public async Task ValidStructuredOutput_UsesServerKeyAndKeepsHumanReview()
    {
        var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""
                {"candidates":[{"content":{"parts":[{"text":"{\"category\":\"delivery\",\"summary\":\"Afternoon delivery\"}"}]}}]}
                """, Encoding.UTF8, "application/json")
        });
        var service = CreateService(handler);

        var result = await service.SuggestAsync("Deliver after 5 PM", AppLanguage.English);

        Assert.Equal("delivery", result.Category);
        Assert.False(result.IsFallback);
        Assert.True(result.RequiresReview);
        Assert.Equal("server-only-test-key", handler.ApiKey);
        Assert.Contains("Deliver after 5 PM", handler.RequestBody);
    }

    [Fact]
    public async Task InvalidCategory_ReturnsSafeFallback()
    {
        var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""
                {"candidates":[{"content":{"parts":[{"text":"{\"category\":\"approve-discount\",\"summary\":\"Done\"}"}]}}]}
                """, Encoding.UTF8, "application/json")
        });

        var result = await CreateService(handler).SuggestAsync("Give me a discount", AppLanguage.English);

        Assert.Equal("other", result.Category);
        Assert.True(result.IsFallback);
        Assert.True(result.RequiresReview);
    }

    [Fact]
    public async Task ProviderFailure_ReturnsSafeFallback()
    {
        var handler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

        var result = await CreateService(handler).SuggestAsync("Deliver after 5 PM", AppLanguage.English);

        Assert.True(result.IsFallback);
        Assert.True(result.RequiresReview);
    }

    [Fact]
    public async Task ProviderTimeout_ReturnsReviewedFallback()
    {
        // An uncancelled caller plus an aborted provider request represents the
        // adapter's linked timeout; no network or ten-second wait is needed.
        var handler = new AsyncHandler((_, token) =>
            throw new OperationCanceledException("Provider timed out", token));

        var result = await CreateService(handler).SuggestAsync("Deliver after 5 PM", AppLanguage.English);

        Assert.Equal("other", result.Category);
        Assert.True(result.IsFallback);
        Assert.True(result.RequiresReview);
        Assert.Contains("No automated suggestion", result.Summary);
    }

    [Fact]
    public async Task CallerCancellation_PropagatesInsteadOfReturningFallback()
    {
        var handler = new AsyncHandler(async (_, token) =>
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            CreateService(handler).SuggestAsync("Deliver after 5 PM", AppLanguage.English, cancellation.Token));
    }

    private static GeminiAiOrderSuggestionService CreateService(HttpMessageHandler handler)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AI:ApiKey"] = "server-only-test-key",
            ["AI:Model"] = "gemini-2.5-flash"
        }).Build();
        return new(new HttpClient(handler), configuration);
    }

    private sealed class RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        public string? ApiKey { get; private set; }
        public string RequestBody { get; private set; } = string.Empty;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ApiKey = request.Headers.GetValues("x-goog-api-key").Single();
            RequestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return respond(request);
        }
    }

    private sealed class AsyncHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) =>
            respond(request, cancellationToken);
    }
}
