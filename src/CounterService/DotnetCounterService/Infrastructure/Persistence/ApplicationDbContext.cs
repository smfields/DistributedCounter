using DistributedCounter.CounterService.Application.Common;
using DistributedCounter.CounterService.Domain.CounterAggregate;
using Microsoft.EntityFrameworkCore;

namespace DistributedCounter.CounterService.Domain.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Counter> Counters { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}