using DistributedCounter.CounterService.API.Common.Grpc;
using DistributedCounter.CounterService.API.Common.Grpc.Errors;
using DistributedCounter.CounterService.API.Counters.ErrorStatusConverters;
using DistributedCounter.CounterService.Domain.Counters.Errors;
using DistributedCounter.CounterService.Utilities.DependencyInjection;

namespace DistributedCounter.CounterService.API.Counters;

public class CounterServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddSingleton<IErrorStatusConverter<CounterOverflowError>, CounterOverflowErrorStatusConverter>();
    }
}