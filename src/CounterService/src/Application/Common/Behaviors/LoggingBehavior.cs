using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DistributedCounter.CounterService.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        using (LogContext.PushProperty("RequestName", requestName))
        using (LogContext.PushProperty("Request", request, true))
        {
            _logger.LogInformation("----- Beginning {RequestName}", requestName);
            var response = await next();
            _logger.LogInformation("----- Completed {RequestName}", requestName);
            return response;
        }
    }
}