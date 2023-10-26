namespace DistributedCounter.CounterService.Domain.Counters.Exceptions;

public class CounterAlreadyInitializedException(Guid counterId) : Exception($"Counter {counterId} has already been initialized")
{
    public Guid CounterId { get; } = counterId;
}