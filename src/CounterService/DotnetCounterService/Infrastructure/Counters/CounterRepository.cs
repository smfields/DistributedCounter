using DistributedCounter.CounterService.Domain.CounterAggregate;

namespace DistributedCounter.CounterService.Domain.Counters;

public class CounterRepository : ICounterRepository
{
    private readonly Dictionary<Guid, Counter> _counters = new();
    
    public Task AddCounterAsync(Counter counter)
    {
        _counters.Add(counter.Id, counter);
        return Task.CompletedTask;
    }

    public Task<Counter> GetCounterAsync(Guid counterId)
    {
        return Task.FromResult(_counters[counterId]);
    }

    public Task UpdateAsync(Counter counter)
    {
        return Task.CompletedTask;
    }
}