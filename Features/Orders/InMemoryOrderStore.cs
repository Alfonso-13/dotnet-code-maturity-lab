using System.Collections.Concurrent;

namespace DotnetCodeMaturityLab.Features.Orders;

public sealed class InMemoryOrderStore : IOrderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task<Order?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_orders.TryGetValue(id, out var order) ? order : null);
    }

    public Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyCollection<Order>>(
            _orders.Values.OrderByDescending(order => order.Id).ToArray());
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_orders.Count);
    }
}
