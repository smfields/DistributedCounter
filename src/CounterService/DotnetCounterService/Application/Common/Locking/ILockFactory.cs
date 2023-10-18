namespace DistributedCounter.CounterService.Application.Common.Locking;

public interface ILockFactory
{
    public Task<ILock> CreateLockAsync(string lockName, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
}