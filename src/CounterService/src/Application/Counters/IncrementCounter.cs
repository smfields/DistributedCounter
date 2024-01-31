using DistributedCounter.CounterService.Domain.Counters;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters;

public static class IncrementCounter
{
    public record Command(Guid CounterId, ulong Amount) : IRequest<Response>;

    public record Response
    {
        public static readonly Response Empty = new();
    }
    
    internal sealed class Handler(IGrainFactory grainFactory) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var counter = grainFactory.GetGrain<ICounter>(command.CounterId);
            var response = await counter.Increment(command.Amount);
            return Response.Empty;
        }
    }
}