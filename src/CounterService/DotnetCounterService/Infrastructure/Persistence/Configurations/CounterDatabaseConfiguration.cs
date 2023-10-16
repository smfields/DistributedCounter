using DistributedCounter.CounterService.Domain.CounterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributedCounter.CounterService.Domain.Persistence.Configurations;

public class CounterDatabaseConfiguration : IEntityTypeConfiguration<Counter>
{
    public void Configure(EntityTypeBuilder<Counter> builder)
    {
        builder.HasKey(counter => counter.Id);
        builder.Ignore(counter => counter.Events);
    }
}