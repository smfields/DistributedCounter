using DistributedCounter.CounterService.Domain.Counters;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters;

public static class CreateCounter
{
    public record Command(long InitialValue) : IRequest<Response>;
    
    public record Response(Guid CounterId);
    
    internal sealed class Handler(IGrainFactory grainFactory) : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            var counterId = Guid.NewGuid();
            var counter = grainFactory.GetGrain<ICounter>(counterId);
            await counter.Initialize(command.InitialValue);
            return new Response(counterId);
        }
    }
}