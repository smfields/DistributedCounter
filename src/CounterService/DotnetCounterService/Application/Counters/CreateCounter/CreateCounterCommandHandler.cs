using DistributedCounter.CounterService.Application.Common;
using DistributedCounter.CounterService.Domain.CounterAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedCounter.CounterService.Application.Counters.CreateCounter;

public record CreateCounterCommand(long InitialValue) : IRequest<CreateCounterResponse>;

public record CreateCounterResponse(Guid CounterId);

public class CreateCounterCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateCounterCommand, CreateCounterResponse>
{
    public async Task<CreateCounterResponse> Handle(CreateCounterCommand command, CancellationToken cancellationToken)
    {
        var counter = new Counter(command.InitialValue);

        await dbContext.Counters.AddAsync(counter, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new CreateCounterResponse(counter.Id);
    }
}