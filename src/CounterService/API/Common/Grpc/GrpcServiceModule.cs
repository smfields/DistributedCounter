using DistributedCounter.CounterService.Utilities.DependencyInjection;

namespace DistributedCounter.CounterService.API.Common.Grpc;

public class GrpcServiceModule : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddGrpc(opts =>
        {
            opts.EnableDetailedErrors = true;
        });
        
        services.AddGrpcReflection();
    }
}