namespace DistributedCounter.CounterService.Domain.Locking;

public class LockDisposedException : Exception
{
    public LockDisposedException() : base("Lock has already been disposed")
    {
    }
}