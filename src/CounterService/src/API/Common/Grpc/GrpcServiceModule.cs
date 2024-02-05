using DistributedCounter.CounterService.API.Common.Grpc.Errors;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.API.Common.Grpc;

public class GrpcServiceModule(IHostEnvironment environment) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddGrpc(opts =>
        {
            opts.EnableDetailedErrors = environment.IsDevelopment();
            opts.Interceptors.Add<FailedResultInterceptor>();
        });
        
        services.AddGrpcReflection();

        services.AddSingleton<IErrorStatusVisitor, ErrorStatusVisitor>();
        services.AddSingleton<IErrorStatusConverter<DetailedError>, DefaultErrorStatusConverter>();
        services.AddSingleton<IErrorStatusConverter<InvalidArgumentError>, InvalidArgumentErrorStatusConverter>();
    }
}