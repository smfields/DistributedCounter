extern alias commonprotos;
using commonprotos::Google.Rpc;
using DistributedCounter.CounterService.Utilities.Errors;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public abstract class BaseErrorStatusConverter<TError> : IErrorStatusConverter<TError> where TError : DetailedError
{
    public virtual Status Convert(TError error)
    {
        return new Status
        {
            Code = GetCode(error),
            Message = GetMessage(error),
            Details =
            {
                Any.Pack(new ErrorInfo
                {
                    Domain = StatusConstants.Domain,
                    Reason = GetReason(error),
                    Metadata =
                    {
                        GetErrorDetailsMetadata(error)
                    }
                }),
                GetAdditionalDetails(error).Select(Any.Pack)
            }
        };
    }

    protected virtual int GetCode(TError error) => error.Type.ToCode();

    protected virtual string GetMessage(TError error) => error.Message;

    protected virtual string GetReason(TError error) => error.Identifier;

    protected virtual Dictionary<string, string> GetErrorDetailsMetadata(TError error) => new();

    protected virtual IEnumerable<IMessage> GetAdditionalDetails(TError error) => Array.Empty<IMessage>();
}