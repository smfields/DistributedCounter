using DistributedCounter.CounterService.Domain.Counters;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.CreateCounter;

public record CreateCounterCommand(long InitialValue) : IRequest<CreateCounterResponse>;

public record CreateCounterResponse(Guid CounterId);

public class CreateCounterCommandHandler(IGrainFactory grainFactory) : IRequestHandler<CreateCounterCommand, CreateCounterResponse>
{
    public async Task<CreateCounterResponse> Handle(CreateCounterCommand command, CancellationToken cancellationToken)
    {
        var counterId = Guid.NewGuid();
        var counter = grainFactory.GetGrain<ICounter>(counterId);
        await counter.Initialize(command.InitialValue);
        return new CreateCounterResponse(counterId);
    }
}