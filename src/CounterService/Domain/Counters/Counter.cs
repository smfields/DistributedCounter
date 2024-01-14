using DistributedCounter.CounterService.Domain.Counters.Events;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.EventSourcing;

namespace DistributedCounter.CounterService.Domain.Counters;

public interface ICounter : IGrainWithGuidKey
{
    [ReadOnly]
    public ValueTask<long> GetCurrentValue();
    public ValueTask Initialize(long initialValue);
    public ValueTask Reset(long newValue);
    public ValueTask Increment(uint amount);
    public ValueTask Decrement(uint amount);
}

public class CounterState
{
    public bool Initialized { get; private set; } = false;
    public long Value { get; private set; }

    public void Apply(CounterInitializedEvent counterInitializedEvent)
    {
        Initialized = true;
        Value = counterInitializedEvent.InitialValue;
    }

    public void Apply(CounterResetEvent counterResetEvent)
    {
        Value = counterResetEvent.NewValue;
    }

    public void Apply(CounterIncrementedEvent counterIncrementedEvent)
    {
        Value += counterIncrementedEvent.Amount;
    }

    public void Apply(CounterDecrementedEvent counterDecrementedEvent)
    {
        Value += counterDecrementedEvent.Amount;
    }
}

[Reentrant]
public class Counter(ILogger<Counter> logger) : JournaledGrain<CounterState>, ICounter
{
    private Guid Id => this.GetPrimaryKey();
    
    public ValueTask<long> GetCurrentValue()
    {
        logger.LogInformation("Current Value of {CounterId} = {CurrentValue}", Id, State.Value);
        return ValueTask.FromResult(State.Value);
    }

    public async ValueTask Initialize(long initialValue)
    {
        logger.LogInformation("Initializing counter {CounterId} to {InitialValue}", Id, initialValue);
        RaiseEvent(new CounterInitializedEvent(initialValue));
        await ConfirmEvents();
    }

    public async ValueTask Reset(long newValue)
    {
        logger.LogInformation("Resetting counter {CounterId} to {NewValue}", Id, newValue);
        RaiseEvent(new CounterResetEvent(newValue));
        await ConfirmEvents();
    }

    public async ValueTask Increment(uint amount)
    {
        logger.LogInformation("Incrementing counter {CounterId} by {IncrementAmount}", Id, amount);
        RaiseEvent(new CounterIncrementedEvent(amount));
        await ConfirmEvents();
    }

    public async ValueTask Decrement(uint amount)
    {
        logger.LogInformation("Decrementing counter {CounterId} by {DecrementAmount}", Id, amount);
        RaiseEvent(new CounterDecrementedEvent(amount));
        await ConfirmEvents();
    }
}