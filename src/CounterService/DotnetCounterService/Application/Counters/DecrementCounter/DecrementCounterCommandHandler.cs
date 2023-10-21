﻿using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.DecrementCounter;

public record DecrementCounterCommand(Guid CounterId, uint Amount) : IRequest<DecrementCounterResponse>;

public record DecrementCounterResponse
{
    public static DecrementCounterResponse Empty = new();
}

public class DecrementCounterCommandHandler(IGrainFactory grainFactory) : IRequestHandler<DecrementCounterCommand, DecrementCounterResponse>
{
    public async Task<DecrementCounterResponse> Handle(DecrementCounterCommand command, CancellationToken cancellationToken)
    {
        var counterClient = new CounterClient(command.CounterId, grainFactory);
        await counterClient.Decrement(command.Amount);
        return DecrementCounterResponse.Empty;
    }
}