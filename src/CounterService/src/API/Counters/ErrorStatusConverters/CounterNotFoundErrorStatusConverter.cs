using DistributedCounter.CounterService.API.Common.Grpc.Errors;
using DistributedCounter.CounterService.Domain.Counters.Errors;
using Google.Protobuf;
using Google.Rpc;

namespace DistributedCounter.CounterService.API.Counters.ErrorStatusConverters;

public class CounterNotFoundErrorStatusConverter : BaseErrorStatusConverter<CounterNotFoundError>
{
    protected override IEnumerable<IMessage> GetAdditionalDetails(CounterNotFoundError error)
    {
        yield return new ResourceInfo
        {
            ResourceType = "Counter",
            ResourceName = error.CounterId.ToString()
        };
    }
}