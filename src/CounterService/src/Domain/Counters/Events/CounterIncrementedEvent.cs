namespace DistributedCounter.CounterService.Domain.Counters.Events;

public record CounterIncrementedEvent(ulong Amount);