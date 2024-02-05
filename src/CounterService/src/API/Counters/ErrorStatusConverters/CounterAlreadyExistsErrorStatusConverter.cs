using DistributedCounter.CounterService.API.Common.Grpc.Errors;
using DistributedCounter.CounterService.Domain.Counters.Errors;
using Google.Protobuf;
using Google.Rpc;

namespace DistributedCounter.CounterService.API.Counters.ErrorStatusConverters;

public class CounterAlreadyExistsErrorStatusConverter : BaseErrorStatusConverter<CounterAlreadyExistsError>
{
    protected override IEnumerable<IMessage> GetAdditionalDetails(CounterAlreadyExistsError error)
    {
        yield return new ResourceInfo
        {
            ResourceType = "Counter",
            ResourceName = error.CounterId.ToString()
        };
    }
}