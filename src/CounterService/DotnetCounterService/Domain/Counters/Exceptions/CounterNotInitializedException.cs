namespace DistributedCounter.CounterService.Domain.Counters.Exceptions;

public class CounterNotInitializedException(Guid counterId) : Exception($"Counter {counterId} has not been initialized yet")
{
    public Guid CounterId { get; } = counterId;
}