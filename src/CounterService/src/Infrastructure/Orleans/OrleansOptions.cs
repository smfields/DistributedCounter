using System.ComponentModel.DataAnnotations;
using DistributedCounter.CounterService.Utilities.Configuration;

namespace DistributedCounter.CounterService.Infrastructure.Orleans;

public class OrleansOptions : IOptions
{
    public static string Section => "Orleans";

    [Required]
    public string EventStoreDbConnectionString { get; init; } = null!;
}