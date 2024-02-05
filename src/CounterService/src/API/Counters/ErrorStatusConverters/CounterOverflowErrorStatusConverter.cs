using DistributedCounter.CounterService.API.Common.Grpc.Errors;
using DistributedCounter.CounterService.Domain.Counters.Errors;
using Google.Protobuf;
using Google.Rpc;

namespace DistributedCounter.CounterService.API.Counters.ErrorStatusConverters;

public class CounterOverflowErrorStatusConverter : BaseErrorStatusConverter<CounterOverflowError>
{
    protected override Dictionary<string, string> GetErrorDetailsMetadata(CounterOverflowError error)
    {
        return new Dictionary<string, string>
        {
            { nameof(CounterOverflowError.CurrentValue), error.CurrentValue.ToString() },
            { nameof(CounterOverflowError.IncrementValue), error.IncrementValue.ToString() }
        };
    }

    protected override IEnumerable<IMessage> GetAdditionalDetails(CounterOverflowError error)
    {
        yield return new ResourceInfo
        {
            ResourceType = "Counter",
            ResourceName = error.CounterId.ToString()
        };
    }
}