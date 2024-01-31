using DistributedCounter.CounterService.Utilities.Reflection;
using FluentResults;

namespace DistributedCounter.CounterService.Infrastructure.Orleans.Converters;

[GenerateSerializer]
public struct ReasonSurrogate
{
    [Id(0)]
    public string Message;

    [Id(1)]
    public Dictionary<string, object> Metadata;

    [Id(2)]
    public List<IError>? Reasons;

    [Id(3)]
    public Exception? Exception;
}

[RegisterConverter]
public sealed class SuccessSurrogateConverter : IConverter<Success, ReasonSurrogate>,
    IPopulator<Success, ReasonSurrogate>
{
    public Success ConvertFromSurrogate(in ReasonSurrogate surrogate)
    {
        return new Success(surrogate.Message)
            .WithMetadata(surrogate.Metadata);
    }

    public ReasonSurrogate ConvertToSurrogate(in Success value)
    {
        return new ReasonSurrogate
        {
            Message = value.Message,
            Metadata = value.Metadata,
            Reasons = null,
            Exception = null
        };
    }

    public void Populate(in ReasonSurrogate surrogate, Success value)
    {
        value.Metadata.Clear();
        value.WithMetadata(surrogate.Metadata);
    }
}

[RegisterConverter]
public sealed class ErrorSurrogateConverter : GenericErrorSurrogateConverter<Error>
{
    protected override Error CreateError(ReasonSurrogate surrogate)
    {
        return new Error(surrogate.Message);
    }
}

public abstract class GenericErrorSurrogateConverter<TError> : 
    IConverter<TError, ReasonSurrogate>,
    IPopulator<TError, ReasonSurrogate> 
    where TError : Error
{
    public TError ConvertFromSurrogate(in ReasonSurrogate surrogate)
    {
        var error = CreateError(surrogate);

        error.CausedBy(surrogate.Reasons);
        error.WithMetadata(surrogate.Metadata);

        return error;
    }

    public ReasonSurrogate ConvertToSurrogate(in TError value)
    {
        return new ReasonSurrogate
        {
            Reasons = value.Reasons,
            Metadata = value.Metadata,
            Message = value.Message,
            Exception = null
        };
    }

    public void Populate(in ReasonSurrogate surrogate, TError value)
    {
        SetPrivateProperty(value, nameof(Error.Message), surrogate.Message);
        SetPrivateProperty(value, nameof(Error.Reasons), surrogate.Reasons);
        SetPrivateProperty(value, nameof(Error.Metadata), surrogate.Metadata);
    }

    private static void SetPrivateProperty(TError error, string propertyName, object? propertyValue)
    {
        var property = typeof(TError).GetProperty(propertyName);

        if (property == null)
        {
            throw new InvalidOperationException($"Failed to find {propertyName} property on Error type");
        }
        
        var propertySetter = ReflectionHelper.GetPropertySetter(property);
        propertySetter.Invoke(error, propertyValue);
    }

    protected abstract TError CreateError(ReasonSurrogate surrogate);
}