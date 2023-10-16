using DistributedCounter.CounterService.Domain.Common;
using DistributedCounter.CounterService.Domain.CounterAggregate.Events;

namespace DistributedCounter.CounterService.Domain.CounterAggregate;

public class Counter : Entity<Guid>
{
    public long Value { get; private set; }

    private Counter() : base(Guid.Empty)
    {
    }
    
    public Counter(long initialValue) : base(Guid.NewGuid())
    {
        Value = initialValue;
        AddDomainEvent(new CounterCreatedEvent(this));
    }

    public void Increment(uint amount)
    {
        Value += amount;
        AddDomainEvent(new CounterIncrementedEvent(this, amount));
    }

    public void Decrement(uint amount)
    {
        Value -= amount;
        AddDomainEvent(new CounterDecrementedEvent(this, amount));
    }
}