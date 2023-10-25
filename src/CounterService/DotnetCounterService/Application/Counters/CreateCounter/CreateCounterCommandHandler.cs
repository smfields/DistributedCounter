using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.CreateCounter;

public record CreateCounterCommand(long InitialValue) : IRequest<CreateCounterResponse>;

public record CreateCounterResponse(Guid CounterId);

public class CreateCounterCommandHandler(CounterClient.Factory counterClientFactory) : IRequestHandler<CreateCounterCommand, CreateCounterResponse>
{
    public async Task<CreateCounterResponse> Handle(CreateCounterCommand command, CancellationToken cancellationToken)
    {
        var counterId = Guid.NewGuid();
        var counterClient = counterClientFactory.CreateClientFor(counterId);
        await counterClient.Initialize(command.InitialValue);
        return new CreateCounterResponse(counterId);
    }
}