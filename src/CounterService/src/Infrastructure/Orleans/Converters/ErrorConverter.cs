using DistributedCounter.CounterService.Utilities.Errors;
using DistributedCounter.CounterService.Utilities.Reflection;
using FluentResults;

namespace DistributedCounter.CounterService.Infrastructure.Orleans.Converters;

[GenerateSerializer]
public struct DetailedErrorSurrogate
{
    [Id(0)]
    public string Message;

    [Id(1)]
    public Dictionary<string, object> Metadata;

    [Id(2)]
    public List<IError>? Reasons;
}

[RegisterConverter]
public class DetailedErrorSurrogateConverter :
    IConverter<DetailedError, DetailedErrorSurrogate>,
    IPopulator<DetailedError, DetailedErrorSurrogate> 
{
    public DetailedError ConvertFromSurrogate(in DetailedErrorSurrogate surrogate)
    {
        throw new InvalidOperationException("Cannot directly instantiate DetailedError");
    }

    public DetailedErrorSurrogate ConvertToSurrogate(in DetailedError value)
    {
        return new DetailedErrorSurrogate
        {
            Reasons = value.Reasons,
            Metadata = value.Metadata,
            Message = value.Message
        };
    }

    public void Populate(in DetailedErrorSurrogate surrogate, DetailedError value)
    {
        SetPrivateProperty(value, nameof(DetailedError.Message), surrogate.Message);
        SetPrivateProperty(value, nameof(DetailedError.Reasons), surrogate.Reasons);
        SetPrivateProperty(value, nameof(DetailedError.Metadata), surrogate.Metadata);
    }
    
    private static void SetPrivateProperty(DetailedError error, string propertyName, object? propertyValue)
    {
        var property = typeof(DetailedError).GetProperty(propertyName);

        if (property == null)
        {
            throw new InvalidOperationException($"Failed to find {propertyName} property on DetailedError type");
        }
        
        var propertySetter = ReflectionHelper.GetPropertySetter(property);
        propertySetter.Invoke(error, propertyValue);
    }
}