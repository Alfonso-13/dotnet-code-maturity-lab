namespace DotnetCodeMaturityLab.Features.Orders;

/// <summary>Pages has no API or SQLite; this is an explicitly labeled walkthrough.</summary>
public sealed class StaticMiddleLabEvidenceService(CreateOrderService service, IOrderStore store) : IMiddleLabEvidenceService
{
    public async Task<MiddleLabEvidence> VerifyAsync(CancellationToken cancellationToken = default)
    {
        // Pages cannot host HTTP endpoints or SQLite. Execute the same domain service
        // against the browser store and label the result as a simulation.
        var result = await service.CreateAsync(new CreateOrderRequest(
            $"DEMO-{Guid.NewGuid():N}", 2, 12.50m), cancellationToken);
        if (!result.IsSuccess || result.Order is null) return new(false, "DEMO SIMULATION: order creation failed.");
        var saved = await store.FindAsync(result.Order.Id, cancellationToken);
        var countBefore = await store.CountAsync(cancellationToken);
        var invalid = await service.CreateAsync(new CreateOrderRequest("", 0, 0), cancellationToken);
        var countAfter = await store.CountAsync(cancellationToken);
        var passed = saved?.Id == result.Order.Id && saved.Subtotal == 25m && !invalid.IsSuccess && countBefore == countAfter;
        return new(passed, passed
            ? "DEMO SIMULATION: domain service created and read back an order in browser memory; invalid input changed no data. No HTTP or SQLite was used."
            : "DEMO SIMULATION: browser-memory validation failed.");
    }
}
