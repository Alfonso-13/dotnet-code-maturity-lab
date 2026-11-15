namespace DotnetCodeMaturityLab.Features.Orders;

public interface IOrderStore
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task<Order?> FindAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
