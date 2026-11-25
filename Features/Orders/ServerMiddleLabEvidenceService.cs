using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace DotnetCodeMaturityLab.Features.Orders;

/// <summary>
/// Exercises the running local API and reads the created row back through HTTP.
/// The server API uses SqliteOrderStore, so this verifies the real persistence path.
/// </summary>
public sealed class ServerMiddleLabEvidenceService(NavigationManager navigation) : IMiddleLabEvidenceService
{
    public async Task<MiddleLabEvidence> VerifyAsync(CancellationToken cancellationToken = default)
    {
        var productCode = $"LAB-{Guid.NewGuid():N}";
        try
        {
            using var client = LoopbackOrderHttpClient.Create(navigation);
            using var createdResponse = await client.PostAsJsonAsync(
                "api/orders", new CreateOrderRequest(productCode, 2, 12.50m), cancellationToken);
            if (createdResponse.StatusCode != HttpStatusCode.Created || createdResponse.Headers.Location is null)
                return new(false, $"Expected 201 and Location; received {(int)createdResponse.StatusCode}.");

            var created = await createdResponse.Content.ReadFromJsonAsync<CreateOrderResponse>(cancellationToken);
            if (created is null || created.Subtotal != 25m)
                return new(false, "Created order did not contain the expected subtotal.");

            // Never follow a response-provided absolute Location from a server-side client.
            // Verify the contract, then use the known relative endpoint for readback.
            var expectedLocation = $"/api/orders/{created.Id}";
            if (createdResponse.Headers.Location.OriginalString != expectedLocation)
                return new(false, "Created order returned an unexpected Location header.");

            using var readResponse = await client.GetAsync($"api/orders/{created.Id}", cancellationToken);
            if (readResponse.StatusCode != HttpStatusCode.OK)
                return new(false, $"Expected readback 200; received {(int)readResponse.StatusCode}.");
            var readback = await readResponse.Content.ReadFromJsonAsync<CreateOrderResponse>(cancellationToken);
            if (readback?.Id != created.Id || readback.ProductCode != productCode)
                return new(false, "The created SQLite order could not be read back through HTTP.");

            using var invalidResponse = await client.PostAsJsonAsync(
                "api/orders", new CreateOrderRequest("", 0, 0m), cancellationToken);
            if (invalidResponse.StatusCode != HttpStatusCode.BadRequest)
                return new(false, $"Expected validation 400; received {(int)invalidResponse.StatusCode}.");

            return new(true, $"LIVE API: 201 Created + Location; GET 200 confirms SQLite order {created.Id}; invalid POST 400.");
        }
        catch (InvalidOperationException)
        {
            return new(false, "The local API check requires a loopback URL. Open the app through start-local.cmd.");
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException)
        {
            return new(false, "The local HTTP check could not reach the API. Check the server URL and try again.");
        }
    }
}
