using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterUnderflowError(Guid counterId, long currentValue, ulong decrementValue) : OutOfRangeError($"Cannot decrement counter ({counterId}) by {decrementValue} because it would cause an underflow.")
{
    [Id(0)]
    public Guid CounterId { get; } = counterId;
    [Id(1)]
    public long CurrentValue { get; } = currentValue;
    [Id(2)]
    public ulong DecrementValue { get; } = decrementValue;
}