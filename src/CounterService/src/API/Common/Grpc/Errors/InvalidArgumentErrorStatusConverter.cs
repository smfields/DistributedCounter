extern alias commonprotos;
using commonprotos::Google.Rpc;
using Google.Protobuf;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public class InvalidArgumentErrorStatusConverter : BaseErrorStatusConverter<InvalidArgumentError>
{
    protected override IEnumerable<IMessage> GetAdditionalDetails(InvalidArgumentError error)
    {
        yield return new BadRequest
        {
            FieldViolations =
            {
                GetFieldViolations(error)
            }
        };
    }

    private IEnumerable<BadRequest.Types.FieldViolation> GetFieldViolations(InvalidArgumentError error)
    {
        foreach (var (field, description) in error.FieldViolations)
        {
            yield return new BadRequest.Types.FieldViolation()
            {
                Field = field,
                Description = description
            };
        }
    }
}