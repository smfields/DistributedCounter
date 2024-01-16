namespace DistributedCounter.CounterService.Domain.Counters.Events;

public record CounterIncrementedEvent(uint Amount);