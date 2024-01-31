using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterAlreadyInitializedError(Guid counterId) : AlreadyExistsError($"Counter ({counterId}) has already been initialized")
{
    [Id(0)]
    public Guid CounterId { get; } = counterId;
}