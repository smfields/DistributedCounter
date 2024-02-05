using System.Numerics;
using DistributedCounter.CounterService.Domain.Counters.Errors;
using DistributedCounter.CounterService.Domain.Counters.Events;
using FluentResults;
using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.EventSourcing;

namespace DistributedCounter.CounterService.Domain.Counters;

public interface ICounter : IGrainWithGuidKey
{
    [ReadOnly]
    public ValueTask<Result<long>> GetCurrentValue();
    public ValueTask<Result> Initialize(long initialValue);
    public ValueTask<Result> Reset(long newValue);
    public ValueTask<Result> Increment(ulong amount);
    public ValueTask<Result> Decrement(ulong amount);
}

public class CounterState
{
    public bool Initialized { get; private set; }
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
        BigInteger currentValue = Value;
        BigInteger incrementValue = counterIncrementedEvent.Amount;
        Value = (long)(currentValue + incrementValue);
    }

    public void Apply(CounterDecrementedEvent counterDecrementedEvent)
    {
        BigInteger currentValue = Value;
        BigInteger decrementValue = counterDecrementedEvent.Amount;
        Value = (long)(currentValue - decrementValue);
    }
}

public class Counter(ILogger<Counter> logger) : JournaledGrain<CounterState>, ICounter
{
    private Guid Id => this.GetPrimaryKey();
    
    public ValueTask<Result<long>> GetCurrentValue()
    {
        logger.LogInformation("Current Value of {CounterId} = {CurrentValue}", Id, State.Value);
        
        if (!State.Initialized)
        {
            return ValueTask.FromResult(Result.Fail<long>(new CounterNotFoundError(Id)));
        }
        
        return ValueTask.FromResult(Result.Ok(State.Value));
    }

    public async ValueTask<Result> Initialize(long initialValue)
    {
        logger.LogInformation("Initializing counter {CounterId} to {InitialValue}", Id, initialValue);

        if (State.Initialized)
        {
            return new CounterAlreadyExistsError(Id);
        }
        
        RaiseEvent(new CounterInitializedEvent(initialValue));
        await ConfirmEvents();
        
        return Result.Ok();
    }

    public async ValueTask<Result> Reset(long newValue)
    {
        logger.LogInformation("Resetting counter {CounterId} to {NewValue}", Id, newValue);
        
        if (!State.Initialized)
        {
            return new CounterNotCreatedError(Id);
        }
        
        RaiseEvent(new CounterResetEvent(newValue));
        await ConfirmEvents();
        
        return Result.Ok();
    }

    public async ValueTask<Result> Increment(ulong amount)
    {
        logger.LogInformation("Incrementing counter {CounterId} by {IncrementAmount}", Id, amount);
        
        if (!State.Initialized)
        {
            return new CounterNotCreatedError(Id);
        }

        if (WouldOverflow(amount))
        {
            return new CounterOverflowError(Id, State.Value, amount);
        }
        
        RaiseEvent(new CounterIncrementedEvent(amount));
        await ConfirmEvents();
        
        return Result.Ok();
    }

    public async ValueTask<Result> Decrement(ulong amount)
    {
        logger.LogInformation("Decrementing counter {CounterId} by {DecrementAmount}", Id, amount);
        
        if (!State.Initialized)
        {
            return new CounterNotCreatedError(Id);
        }
        
        if (WouldUnderflow(amount))
        {
            return new CounterUnderflowError(Id, State.Value, amount);
        }
        
        RaiseEvent(new CounterDecrementedEvent(amount));
        await ConfirmEvents();
        
        return Result.Ok();
    }

    private bool WouldOverflow(ulong incrementAmount)
    {
        BigInteger currentValue = State.Value;
        BigInteger incrementValue = incrementAmount;
        return currentValue + incrementValue > long.MaxValue;
    }

    private bool WouldUnderflow(ulong decrementAmount)
    {
        BigInteger currentValue = State.Value;
        BigInteger decrementValue = decrementAmount;
        return currentValue - decrementValue < long.MinValue;
    }
}