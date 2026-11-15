using Microsoft.EntityFrameworkCore;

namespace DotnetCodeMaturityLab.Features.Orders;

public sealed class SqliteOrderStore(IDbContextFactory<OrderDbContext> contextFactory)
    : IOrderStore
{
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await using var database = await contextFactory.CreateDbContextAsync(cancellationToken);
        database.Orders.Add(order);
        await database.SaveChangesAsync(cancellationToken);
    }

    public async Task<Order?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var database = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await database.Orders
            .AsNoTracking()
            .SingleOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var database = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await database.Orders
            .AsNoTracking()
            .OrderByDescending(order => order.Id)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        await using var database = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await database.Orders.CountAsync(cancellationToken);
    }
}
