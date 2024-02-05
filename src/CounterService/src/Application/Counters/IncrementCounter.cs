using DistributedCounter.CounterService.Domain.Counters;
using FluentResults;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters;

public static class IncrementCounter
{
    public record Command(Guid CounterId, ulong Amount) : IRequest<Response>;

    public record Response(Result Result);
    
    internal sealed class Handler(IGrainFactory grainFactory) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var counter = grainFactory.GetGrain<ICounter>(command.CounterId);
            var result = await counter.Increment(command.Amount);
            return new Response(result);
        }
    }
}