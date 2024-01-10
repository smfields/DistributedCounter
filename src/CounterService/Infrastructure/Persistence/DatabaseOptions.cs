using DistributedCounter.CounterService.Utilities.Configuration;

namespace DistributedCounter.CounterService.Domain.Persistence;

public class DatabaseOptions : IOptions
{
    public static string Section => "Database";

    public string ConnectionString { get; set; } = null!;
}