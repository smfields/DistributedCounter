using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterNotInitializedError(Guid counterId)
    : FailedPreconditionError($"Counter ({counterId}) has not been initialized yet")
{
    [Id(0)]
    public Guid CounterId { get; } = counterId;
}