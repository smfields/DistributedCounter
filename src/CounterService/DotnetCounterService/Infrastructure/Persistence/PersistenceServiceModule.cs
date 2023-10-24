using DistributedCounter.CounterService.Application.Common;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCounter.CounterService.Domain.Persistence;

public class PersistenceServiceModule(IConfiguration configuration) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(opts => 
            opts.UseNpgsql(configuration.GetConnectionString("Database"))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
        );
        
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }
}