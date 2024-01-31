using FluentResults;

namespace DistributedCounter.CounterService.Infrastructure.Orleans.Converters;

[GenerateSerializer]
public struct ResultSurrogate
{
    [Id(0)]
    public List<IReason> Reasons;
}

[GenerateSerializer]
public struct ResultSurrogate<TValue>
{
    [Id(0)]
    public List<IReason> Reasons;

    [Id(1)]
    public TValue? Value;
}


[RegisterConverter]
public sealed class ResultConverter :
    IConverter<Result, ResultSurrogate>
{
    public Result ConvertFromSurrogate(in ResultSurrogate surrogate)
    {
        return new Result().WithReasons(surrogate.Reasons);
    }

    public ResultSurrogate ConvertToSurrogate(in Result value)
    {
        return new ResultSurrogate() { Reasons = value.Reasons };
    }
}


[RegisterConverter]
public sealed class ResultConverter<TValue> :
    IConverter<Result<TValue>, ResultSurrogate<TValue>>
{
    public Result<TValue> ConvertFromSurrogate(in ResultSurrogate<TValue> surrogate)
    {
        var result = new Result<TValue>().WithReasons(surrogate.Reasons);
        if (surrogate.Value is not null)
        {
            result.WithValue(surrogate.Value);
        }
        return result;
    }

    public ResultSurrogate<TValue> ConvertToSurrogate(in Result<TValue> value)
    {
        return new ResultSurrogate<TValue>() { Reasons = value.Reasons, Value = value.ValueOrDefault };
    }
}