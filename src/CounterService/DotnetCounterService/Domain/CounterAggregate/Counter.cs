namespace DistributedCounter.CounterService.Domain.CounterAggregate;

public interface ICounter : IGrainWithGuidKey
{
    public ValueTask<long> GetCurrentValue();
    public ValueTask Initialize(long initialValue);
    public ValueTask Increment(uint amount);
    public ValueTask Decrement(uint amount);
}

public class Counter : ICounter
{
    public long Value { get; private set; }

    public ValueTask<long> GetCurrentValue()
    {
        return ValueTask.FromResult(Value);
    }

    public ValueTask Initialize(long initialValue)
    {
        Value = initialValue;
        return ValueTask.CompletedTask;
    }

    public ValueTask Increment(uint amount)
    {
        Value += amount;
        return ValueTask.CompletedTask;
    }

    public ValueTask Decrement(uint amount)
    {
        Value -= amount;
        return ValueTask.CompletedTask;
    }
}