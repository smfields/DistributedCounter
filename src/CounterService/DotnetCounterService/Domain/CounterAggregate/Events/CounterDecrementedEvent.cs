using DistributedCounter.CounterService.Domain.Common;

namespace DistributedCounter.CounterService.Domain.CounterAggregate.Events;

public record CounterDecrementedEvent(Counter Counter, uint Amount) : DomainEvent;