using MediatR;
using Microsoft.Extensions.Logging;

namespace DistributedCounter.CounterService.Application.Common.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>(
        ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger
    )
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = typeof(TRequest).Name;
            logger.LogError(e, "Unhandled Exception for Request {Name} {@Request}", requestName, request);
            throw;
        }
    }
}