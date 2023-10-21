using Orleans.Concurrency;
using Orleans.Runtime;

namespace DistributedCounter.CounterService.Domain.CounterAggregate;

public interface ICounterShard : IGrainWithGuidCompoundKey
{
    public ValueTask<long> GetCurrentValue();
    public ValueTask Initialize(long initialValue);
    public ValueTask Increment(uint amount);
    public ValueTask Decrement(uint amount);
}

public class CounterShardState
{
    public long Value { get; set; }
}

public class CounterShard([PersistentState("counter")]IPersistentState<CounterShardState> counter) : ICounterShard
{
    [ReadOnly]
    public ValueTask<long> GetCurrentValue()
    {
        return ValueTask.FromResult(counter.State.Value);
    }

    public async ValueTask Initialize(long initialValue)
    {
        counter.State.Value = initialValue;
        await counter.WriteStateAsync();
    }

    public async ValueTask Increment(uint amount)
    {
        counter.State.Value += amount;
        await counter.WriteStateAsync();
    }

    public async ValueTask Decrement(uint amount)
    {
        counter.State.Value += amount;
        await counter.WriteStateAsync();
    }
}