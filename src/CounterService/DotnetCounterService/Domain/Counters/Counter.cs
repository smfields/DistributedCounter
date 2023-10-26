using DistributedCounter.CounterService.Domain.Counters.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace DistributedCounter.CounterService.Domain.Counters;

public interface ICounter : IGrainWithGuidKey
{
    [ReadOnly]
    public ValueTask<long> GetCurrentValue();
    public ValueTask Initialize(long initialValue);
    [AlwaysInterleave]
    public ValueTask Increment(uint amount);
    [AlwaysInterleave]
    public ValueTask Decrement(uint amount);
}

public class CounterState
{
    public List<Guid> ShardGrains { get; set; } = new();
}

public class Counter(
    IGrainFactory grainFactory,
    [PersistentState("counter")]IPersistentState<CounterState> counterState,
    IOptions<CounterOptions> options,
    ILogger<Counter> logger
) : Grain, ICounter
{
    private Guid Id => this.GetPrimaryKey();
    private IReadOnlyList<ICounterShard> Shards { get; set; } = null!;
    
    private readonly Random _random = new();

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        UpdateLocalShards();
        return Task.CompletedTask;
    }
    
    public async ValueTask<long> GetCurrentValue()
    {
        EnsureInitialized();
        
        logger.LogDebug("Gathering current counter value for {CounterId}", Id);
        
        var tasks = Shards.Select(async shard => await shard.GetCurrentValue());
        var values = await Task.WhenAll(tasks);
        var sum = values.Sum();
        
        logger.LogInformation("Current Value of {CounterId} = {CurrentValue}", Id, sum);
        
        return sum;
    }

    public async ValueTask Initialize(long initialValue)
    {
        EnsureNotInitialized();
        logger.LogInformation("Initializing counter {CounterId} to {InitialValue}", Id, initialValue);
        await AddShards(options.Value.InitialShardCount);
        await RandomShard().SetValue(initialValue);
    }

    public async ValueTask Increment(uint amount)
    {
        EnsureInitialized();
        logger.LogInformation("Incrementing counter {CounterId} by {IncrementAmount}", Id, amount);
        await RandomShard().Increment(amount);
    }

    public async ValueTask Decrement(uint amount)
    {
        EnsureInitialized();
        logger.LogInformation("Decrementing counter {CounterId} by {DecrementAmount}", Id, amount);
        await RandomShard().Decrement(amount);
    }

    private ICounterShard RandomShard()
    {
        var randomIndex = _random.Next(Shards.Count);
        return Shards[randomIndex];
    }

    private async Task AddShards(int count)
    {
        for (var i = 0; i < count; i++)
        {
            counterState.State.ShardGrains.Add(Guid.NewGuid());
        }
        
        await counterState.WriteStateAsync();
        UpdateLocalShards();
    }

    private void UpdateLocalShards()
    {
        Shards = counterState.State.ShardGrains
            .Select(id => grainFactory.GetGrain<ICounterShard>(id))
            .ToList()
            .AsReadOnly();
    }

    private void EnsureInitialized()
    {
        if (!counterState.RecordExists)
        {
            throw new CounterNotInitializedException(this.GetPrimaryKey());
        }
    }
    
    private void EnsureNotInitialized()
    {
        if (counterState.RecordExists)
        {
            throw new CounterAlreadyInitializedException(this.GetPrimaryKey());
        }
    }
}