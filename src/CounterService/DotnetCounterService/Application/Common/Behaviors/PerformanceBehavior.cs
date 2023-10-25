using System.Diagnostics;
using DistributedCounter.CounterService.Utilities.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DistributedCounter.CounterService.Application.Common.Behaviors;

public class PerformanceBehaviourOptions : IOptions
{
    public static string Section =>  "PerformanceBehaviour";

    public long LongRunningRequestThresholdInMs { get; set; } = 500;
}

public class PerformanceBehavior<TRequest, TResponse>(
        ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
        IOptionsSnapshot<PerformanceBehaviourOptions> configuration
    )
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        var warningThreshold = configuration.Value.LongRunningRequestThresholdInMs;
        var logLevel = elapsedMilliseconds > warningThreshold ? LogLevel.Warning : LogLevel.Information;
        logger.Log(logLevel, "Request completed in {ElapsedMilliseconds} milliseconds", elapsedMilliseconds);

        return response;
    }
}