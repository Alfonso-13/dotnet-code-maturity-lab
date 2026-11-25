using System.Net.Http.Json;
using System.Text.Json;
using DotnetCodeMaturityLab.Features.Localization;
using Microsoft.AspNetCore.Components;

namespace DotnetCodeMaturityLab.Features.Orders;

/// <summary>Uses the running ASP.NET Core endpoint, including its real SQLite persistence.</summary>
public sealed class ServerOrderLabTransport(NavigationManager navigation) : IOrderLabTransport
{
    public async Task<OrderLabSubmission> SubmitAsync(
        CreateOrderRequest request, AppLanguage language, CancellationToken cancellationToken = default)
    {
        using var client = LoopbackOrderHttpClient.Create(navigation);
        using var message = new HttpRequestMessage(HttpMethod.Post, "api/orders")
        {
            Content = JsonContent.Create(request)
        };
        message.Headers.AcceptLanguage.ParseAdd(language == AppLanguage.Spanish ? "es" : "en");
        using var response = await client.SendAsync(message, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        var created = response.StatusCode == System.Net.HttpStatusCode.Created
            ? JsonSerializer.Deserialize<CreateOrderResponse>(body, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            : null;
        return new((int)response.StatusCode, body, created);
    }
}
