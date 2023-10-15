namespace DistributedCounter.CounterService.Domain.CounterAggregate;

public interface ICounterRepository
{
    public Task AddCounterAsync(Counter counter);
    public Task<Counter> GetCounterAsync(Guid counterId);
    public Task UpdateAsync(Counter counter);
}