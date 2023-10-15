using DistributedCounter.CounterService.Domain.Common;

namespace DistributedCounter.CounterService.Domain.CounterAggregate.Events;

public record CounterIncrementedEvent(Counter Counter, uint Amount) : DomainEvent;