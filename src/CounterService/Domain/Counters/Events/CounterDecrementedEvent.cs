namespace DistributedCounter.CounterService.Domain.Counters.Events;

public record CounterDecrementedEvent(uint Amount);