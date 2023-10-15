using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;

namespace DistributedCounter.CounterService.Application.Counters.CreateCounter;

public record CreateCounterCommand(long InitialValue) : IRequest<CreateCounterResponse>;

public record CreateCounterResponse(Guid CounterId);

public class CreateCounterCommandHandler : IRequestHandler<CreateCounterCommand, CreateCounterResponse>
{
    private readonly ICounterRepository _counterRepository;

    public CreateCounterCommandHandler(ICounterRepository counterRepository)
    {
        _counterRepository = counterRepository;
    }

    public async Task<CreateCounterResponse> Handle(CreateCounterCommand command, CancellationToken cancellationToken)
    {
        var counter = new Counter(command.InitialValue);
        await _counterRepository.AddCounterAsync(counter);
        return new CreateCounterResponse(counter.Id);
    }
}