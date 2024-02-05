extern alias commonprotos;
using commonprotos::Google.Rpc;
using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public interface IErrorStatusConverter<in TError> where TError : DetailedError
{
    public Status Convert(TError error);
}