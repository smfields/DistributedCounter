extern alias commonprotos;
using DistributedCounter.CounterService.Utilities.Errors;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public class FailedResultInterceptor(IErrorStatusVisitor errorStatusVisitor) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation
    )
    {
        try
        {
            return await continuation(request, context);
        }
        catch (FailedResultException ex)
        {
            var result = ex.Result;
            var primaryError = result.GetPrimaryError();

            if (primaryError is DetailedError detailedError)
            {
                detailedError.Accept(errorStatusVisitor);
                throw errorStatusVisitor.Status.ToRpcException();
            }

            throw;
        }
    }
}