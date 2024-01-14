using System.ComponentModel.DataAnnotations;
using DistributedCounter.CounterService.Utilities.Configuration;

namespace DistributedCounter.CounterService.Infrastructure.Telemetry;

public class ZipkinOptions : IOptions
{
    public static string Section => "Zipkin";
    
    public bool Enabled { get; init; } = true;
    
    [Required]
    public string Endpoint { get; init; } = null!;
}