using DistributedCounter.CounterService.Utilities.Reflection;
using FluentResults;

namespace DistributedCounter.CounterService.Infrastructure.Orleans.Converters;

[GenerateSerializer]
public struct SuccessSurrogate
{
    [Id(0)]
    public string Message;

    [Id(1)]
    public Dictionary<string, object> Metadata;
}

[RegisterConverter]
public sealed class SuccessSurrogateConverter : IConverter<Success, SuccessSurrogate>,
    IPopulator<Success, SuccessSurrogate>
{
    public Success ConvertFromSurrogate(in SuccessSurrogate surrogate)
    {
        return new Success(surrogate.Message)
            .WithMetadata(surrogate.Metadata);
    }

    public SuccessSurrogate ConvertToSurrogate(in Success value)
    {
        return new SuccessSurrogate
        {
            Message = value.Message,
            Metadata = value.Metadata
        };
    }

    public void Populate(in SuccessSurrogate surrogate, Success value)
    {
        value.Metadata.Clear();
        value.WithMetadata(surrogate.Metadata);
    }
}