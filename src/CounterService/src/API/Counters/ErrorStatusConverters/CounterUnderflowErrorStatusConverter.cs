extern alias commonprotos;
using commonprotos::Google.Rpc;
using DistributedCounter.CounterService.API.Common.Grpc.Errors;
using DistributedCounter.CounterService.Domain.Counters.Errors;
using Google.Protobuf;

namespace DistributedCounter.CounterService.API.Counters.ErrorStatusConverters;

public class CounterUnderflowErrorStatusConverter : BaseErrorStatusConverter<CounterUnderflowError>
{
    protected override Dictionary<string, string> GetErrorDetailsMetadata(CounterUnderflowError error)
    {
        return new Dictionary<string, string>
        {
            { nameof(CounterUnderflowError.CurrentValue), error.CurrentValue.ToString() },
            { nameof(CounterUnderflowError.DecrementValue), error.DecrementValue.ToString() }
        };
    }
    
    protected override IEnumerable<IMessage> GetAdditionalDetails(CounterUnderflowError error)
    {
        yield return new ResourceInfo
        {
            ResourceType = "Counter",
            ResourceName = error.CounterId.ToString()
        };
    }
}