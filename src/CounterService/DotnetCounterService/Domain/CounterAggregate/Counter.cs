namespace DistributedCounter.CounterService.Domain.CounterAggregate;

public interface ICounterClient
{
    public ValueTask<long> GetCurrentValue();
    public ValueTask Initialize(long initialValue);
    public ValueTask Increment(uint amount);
    public ValueTask Decrement(uint amount);
}

public class CounterClient : ICounterClient
{
    private const int ShardCount = 100;
    private readonly ICounterShard[] _shards = new ICounterShard[ShardCount];
    private readonly Random _random = new();

    public CounterClient(
        Guid id,
        IGrainFactory grainFactory
    )
    {
        for (var i = 0; i < ShardCount; i++)
        {
            _shards[i] = grainFactory.GetGrain<ICounterShard>(id, i.ToString());
        }
    }
    
    public async ValueTask<long> GetCurrentValue()
    {
        var tasks = _shards.Select(async shard => await shard.GetCurrentValue());
        var values = await Task.WhenAll(tasks);
        return values.Sum();
    }

    public async ValueTask Initialize(long initialValue)
    {
        await RandomShard().Initialize(initialValue);
    }

    public async ValueTask Increment(uint amount)
    {
        await RandomShard().Increment(amount);
    }

    public async ValueTask Decrement(uint amount)
    {
        await RandomShard().Decrement(amount);
    }

    private ICounterShard RandomShard()
    {
        return _random.GetItems(_shards, 1)[0];
    }
}