using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterNotFoundError(Guid counterId) : NotFoundError($"Could not find counter with id: {counterId}")
{
    [Id(0)]
    public Guid CounterId { get; } = counterId;
}