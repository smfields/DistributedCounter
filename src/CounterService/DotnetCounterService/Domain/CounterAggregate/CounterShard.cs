using Microsoft.Extensions.Logging;
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

public class CounterShard(
        [PersistentState("counter")]IPersistentState<CounterShardState> counter,
        ILogger<CounterShard> logger
    ) 
    : Grain, ICounterShard
{
    [ReadOnly]
    public ValueTask<long> GetCurrentValue()
    {
        var currentValue = counter.State.Value;
        logger.LogDebug("Counter shard {CounterShardId} current value = {CurrentValue}", this.GetPrimaryKeyString(), currentValue);
        return ValueTask.FromResult(currentValue);
    }

    public async ValueTask Initialize(long initialValue)
    {
        logger.LogDebug("Initializing counter shard {CounterShardId} to {InitialValue}", this.GetPrimaryKeyString(), initialValue);
        counter.State.Value = initialValue;
        await counter.WriteStateAsync();
    }

    public async ValueTask Increment(uint amount)
    {
        logger.LogDebug("Incrementing counter shard {CounterShardId} by {IncrementAmount}", this.GetPrimaryKeyString(), amount);
        counter.State.Value += amount;
        await counter.WriteStateAsync();
    }

    public async ValueTask Decrement(uint amount)
    {
        logger.LogDebug("Decrementing counter shard {CounterShardId} by {DecrementAmount}", this.GetPrimaryKeyString(), amount);
        counter.State.Value -= amount;
        await counter.WriteStateAsync();
    }
}