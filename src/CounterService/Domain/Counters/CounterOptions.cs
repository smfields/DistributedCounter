using DistributedCounter.CounterService.Utilities.Configuration;

namespace DistributedCounter.CounterService.Domain.Counters;

public class CounterOptions : IOptions
{
    public static string Section => "Counter";

    public int InitialShardCount { get; set; } = 100;
}