using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterAlreadyExistsError(Guid counterId) : DetailedError($"Counter ({counterId}) has already been initialized")
{
    public override ErrorType Type => ErrorType.AlreadyExists;

    public override string Identifier => "COUNTER_ALREADY_EXISTS";

    [Id(1)]
    public Guid CounterId { get; } = counterId;

    public override void Accept(IErrorVisitor visitor)
    {
        visitor.Visit(this);
    }
}