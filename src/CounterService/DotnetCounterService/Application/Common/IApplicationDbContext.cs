using DistributedCounter.CounterService.Domain.CounterAggregate;
using Microsoft.EntityFrameworkCore;

namespace DistributedCounter.CounterService.Application.Common;

public interface IApplicationDbContext
{
    public DbSet<Counter> Counters { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}