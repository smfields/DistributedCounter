using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterUnderflowError(Guid counterId, long currentValue, ulong decrementValue) : DetailedError($"Cannot decrement counter ({counterId}) by {decrementValue} because it would cause an underflow.")
{
    public override ErrorType Type => ErrorType.OutOfRange;

    public override string Identifier => "COUNTER_UNDERFLOW";

    [Id(0)]
    public Guid CounterId { get; } = counterId;
    
    [Id(1)]
    public long CurrentValue { get; } = currentValue;
    
    [Id(2)]
    public ulong DecrementValue { get; } = decrementValue;
    
    public override void Accept(IErrorVisitor visitor)
    {
        visitor.Visit(this);
    }
}