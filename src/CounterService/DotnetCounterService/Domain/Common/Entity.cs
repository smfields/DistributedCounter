namespace DistributedCounter.CounterService.Domain.Common;

public abstract class Entity<T>
{
    public T Id { get; }

    public IReadOnlyList<DomainEvent> Events => _events.AsReadOnly();
    private readonly List<DomainEvent> _events = new();
    
    protected Entity(T id)
    {
        Id = id;
    }

    protected void AddDomainEvent(DomainEvent @event)
    {
        _events.Add(@event);
    }
}