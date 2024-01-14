namespace DistributedCounter.CounterService.Domain.Counters.Events;

public record CounterResetEvent(long NewValue);