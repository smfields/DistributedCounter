﻿using DistributedCounter.CounterService.Domain.Persistence;
using DistributedCounter.CounterService.Utilities.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DistributedCounter.CounterService.Domain.Orleans;

public class OrleansServiceModule(IConfiguration configuration, IHostEnvironment environment) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        var databaseOptions = configuration.GetOptions<DatabaseOptions>();
        
        services.AddOrleans(siloBuilder =>
        {
            siloBuilder
                .AddActivityPropagation()
                .AddMemoryGrainStorageAsDefault();

            if (environment.IsDevelopment())
            {
                siloBuilder.UseLocalhostClustering();
            }
            else
            {
                
            }
        });
    }
}