using DistributedCounter.CounterService.Domain.Counters;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters;

public static class ResetCounter
{
    public record Command(Guid CounterId, long NewValue) : IRequest<Response>;

    public record Response;
    
    internal sealed class Handler(IGrainFactory grainFactory) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var counter = grainFactory.GetGrain<ICounter>(command.CounterId);
            await counter.Reset(command.NewValue);
            return new Response();
        }
    }
}