using DistributedCounter.CounterService.Application.Common.Locking;
using RedLockNet;

namespace DistributedCounter.CounterService.Domain.Locking;

public class RedLock : ILock
{
    private readonly IRedLock _redLock;

    public RedLock(IRedLock redLock)
    {
        _redLock = redLock;
    }

    internal event EventHandler? Disposed;

    public bool IsAcquired => _redLock.IsAcquired;
    public string LockName => _redLock.LockId;
    private int AcquireDepth { get; set; } = 1;
    private bool IsDisposed { get; set; }

    public void Reacquire()
    {
        if (IsDisposed)
        {
            throw new Exception("Cannot reacquire a disposed lock.");
        }

        AcquireDepth++;
    }

    public async ValueTask DisposeAsync()
    {
        if (AcquireDepth == 1)
        {
            await _redLock.DisposeAsync();
            IsDisposed = true;
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        AcquireDepth--;
    }
}