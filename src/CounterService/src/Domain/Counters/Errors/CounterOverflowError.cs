using DistributedCounter.CounterService.Utilities.Errors;
using FluentResults;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterOverflowError(Guid counterId, long currentValue, ulong incrementValue) : Error($"Cannot increment counter ({counterId}) by {incrementValue} because it would cause an overflow.")
{
    [Id(0)]
    public Guid CounterId { get; } = counterId;
    [Id(1)]
    public long CurrentValue { get; } = currentValue;
    [Id(2)]
    public ulong IncrementValue { get; } = incrementValue;
}