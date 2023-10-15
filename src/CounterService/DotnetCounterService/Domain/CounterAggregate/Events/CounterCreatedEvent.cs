using DistributedCounter.CounterService.Domain.Common;

namespace DistributedCounter.CounterService.Domain.CounterAggregate.Events;

public record CounterCreatedEvent(Counter Counter) : DomainEvent;