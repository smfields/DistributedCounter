﻿using DistributedCounter.CounterService.Utilities.DependencyInjection;
using EventStore.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Clustering.Kubernetes;

namespace DistributedCounter.CounterService.Infrastructure.Orleans;

public class OrleansServiceModule(IConfiguration configuration, IHostEnvironment environment) : ServiceModule
{
    public override void Load(IServiceCollection services)
    {
        var orleansOptions = configuration.GetOptions<OrleansOptions>();
        
        services.AddOrleans(siloBuilder =>
        {
            siloBuilder
                .AddActivityPropagation()
                .AddEventStorageBasedLogConsistencyProviderAsDefault()
                .AddEventStoreEventStorageAsDefault(opts =>
                {
                    opts.ClientSettings = EventStoreClientSettings.Create(orleansOptions.EventStoreDbConnectionString);
                });

            if (environment.IsDevelopment())
            {
                siloBuilder.UseLocalhostClustering();
            }
            else
            {
                siloBuilder
                    .UseKubernetesHosting()
                    .UseKubeMembership();
            }
        });
    }
}