using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterOverflowError(Guid counterId, long currentValue, ulong incrementValue) : DetailedError($"Cannot increment counter ({counterId}) by {incrementValue} because it would cause an overflow.")
{
    public override ErrorType Type => ErrorType.OutOfRange;
    
    public override string Identifier => "COUNTER_OVERFLOW_ERROR";

    [Id(0)]
    public Guid CounterId { get; } = counterId;

    [Id(1)]
    public long CurrentValue { get; } = currentValue;

    [Id(2)]
    public ulong IncrementValue { get; } = incrementValue;
    
    public override void Accept(IErrorVisitor visitor)
    {
        visitor.Visit(this);
    }
}