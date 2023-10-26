using Microsoft.Extensions.Logging;
using Orleans.Concurrency;

namespace DistributedCounter.CounterService.Domain.CounterAggregate;

public interface ICounter : IGrainWithGuidKey
{
    public ValueTask<long> GetCurrentValue();
    public ValueTask Initialize(long initialValue);
    public ValueTask Increment(uint amount);
    public ValueTask Decrement(uint amount);
}

[Reentrant]
public class Counter : Grain, ICounter
{
    private Guid Id => this.GetPrimaryKey();
    
    private readonly ILogger<Counter> _logger;
    private const int ShardCount = 100;
    private readonly ICounterShard[] _shards = new ICounterShard[ShardCount];
    private readonly Random _random = new();

    public Counter(
        IGrainFactory grainFactory,
        ILogger<Counter> logger
    )
    {
        _logger = logger;
        for (var i = 0; i < ShardCount; i++)
        {
            _shards[i] = grainFactory.GetGrain<ICounterShard>(Id, i.ToString());
        }
    }
    
    [ReadOnly]
    public async ValueTask<long> GetCurrentValue()
    {
        _logger.LogDebug("Gathering current counter value for {CounterId}", Id);
        
        var tasks = _shards.Select(async shard => await shard.GetCurrentValue());
        var values = await Task.WhenAll(tasks);
        var sum = values.Sum();
        
        _logger.LogInformation("Current Value of {CounterId} = {CurrentValue}", Id, sum);
        
        return sum;
    }

    public async ValueTask Initialize(long initialValue)
    {
        _logger.LogInformation("Initializing counter {CounterId} to {InitialValue}", Id, initialValue);
        await RandomShard().Initialize(initialValue);
    }

    public async ValueTask Increment(uint amount)
    {
        _logger.LogInformation("Incrementing counter {CounterId} by {IncrementAmount}", Id, amount);
        await RandomShard().Increment(amount);
    }

    public async ValueTask Decrement(uint amount)
    {
        _logger.LogInformation("Decrementing counter {CounterId} by {DecrementAmount}", Id, amount);
        await RandomShard().Decrement(amount);
    }

    private ICounterShard RandomShard()
    {
        return _random.GetItems(_shards, 1)[0];
    }
}