﻿using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Domain.Counters.Errors;

[GenerateSerializer]
public class CounterNotFoundError(Guid counterId) : DetailedError($"Could not find counter with id: {counterId}")
{
    public override ErrorType Type => ErrorType.NotFound;

    public override string Identifier => "COUNTER_NOT_FOUND";

    [Id(0)]
    public Guid CounterId { get; } = counterId;
    
    public override void Accept(IErrorVisitor visitor)
    {
        visitor.Visit(this);
    }
}