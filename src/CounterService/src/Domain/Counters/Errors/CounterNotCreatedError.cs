using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterNotCreatedError(Guid counterId) : DetailedError($"Counter ({counterId}) has not been created yet")
{
    public override ErrorType Type => ErrorType.FailedPrecondition;

    public override string Identifier => "COUNTER_NOT_CREATED";

    [Id(0)]
    public Guid CounterId { get; } = counterId;
    
    public override void Accept(IErrorVisitor visitor)
    {
        visitor.Visit(this);
    }
}