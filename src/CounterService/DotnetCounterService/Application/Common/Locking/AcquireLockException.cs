namespace DistributedCounter.CounterService.Application.Common.Locking;

public class AcquireLockException : Exception
{
    public AcquireLockException() : base("Failed to acquire lock")
    {
    }
}