using DistributedCounter.CounterService.Utilities;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace DistributedCounter.CounterService.API.Common.Grpc.Interceptors;

public class RequestContextInterceptor : Interceptor
{
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        RequestContext.Initialize("gRPC");
        
        return base.UnaryServerHandler(request, context, continuation);
    }
}