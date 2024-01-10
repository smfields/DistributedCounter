using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace DistributedCounter.CounterService.Domain.Counters;

public interface ICounterShard : IGrainWithGuidKey
{
    public ValueTask<long> GetCurrentValue();
    public ValueTask SetValue(long value);
    public ValueTask Increment(uint amount);
    public ValueTask Decrement(uint amount);
}

public class CounterShardState
{
    public long Value { get; set; }
}

public class CounterShard(
        [PersistentState("counter-shard")]IPersistentState<CounterShardState> counterShardState,
        ILogger<CounterShard> logger
    ) 
    : Grain, ICounterShard
{
    [ReadOnly]
    public ValueTask<long> GetCurrentValue()
    {
        var currentValue = counterShardState.State.Value;
        logger.LogDebug("Counter shard {CounterShardId} current value = {CurrentValue}", this.GetPrimaryKeyString(), currentValue);
        return ValueTask.FromResult(currentValue);
    }

    public async ValueTask SetValue(long value)
    {
        logger.LogDebug("Setting counter shard {CounterShardId} to {Value}", this.GetPrimaryKeyString(), value);
        counterShardState.State.Value = value;
        await counterShardState.WriteStateAsync();
    }

    public async ValueTask Increment(uint amount)
    {
        logger.LogDebug("Incrementing counter shard {CounterShardId} by {IncrementAmount}", this.GetPrimaryKeyString(), amount);
        counterShardState.State.Value += amount;
        await counterShardState.WriteStateAsync();
    }

    public async ValueTask Decrement(uint amount)
    {
        logger.LogDebug("Decrementing counter shard {CounterShardId} by {DecrementAmount}", this.GetPrimaryKeyString(), amount);
        counterShardState.State.Value -= amount;
        await counterShardState.WriteStateAsync();
    }
}