namespace DistributedCounter.CounterService.Application.Common.Locking;

public interface ILock : IAsyncDisposable
{
    public bool IsAcquired { get; }
    public string LockName { get; }
}