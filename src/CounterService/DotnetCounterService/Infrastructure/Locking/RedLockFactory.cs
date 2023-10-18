using DistributedCounter.CounterService.Application.Common.Locking;
using Microsoft.Extensions.Logging;
using RedLockNet;

namespace DistributedCounter.CounterService.Domain.Locking;

public class RedLockFactory(
    ILogger<RedLockFactory> logger,
    IDistributedLockFactory redLockFactory
)
    : ILockFactory
{
    private readonly Dictionary<string, RedLock> _heldLocks = new();

    public async Task<ILock> CreateLockAsync(string lockName, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        if (_heldLocks.TryGetValue(lockName, out var redLock))
        {
            return await ReacquireRedLock(redLock, timeout, cancellationToken);
        }

        return await AcquireRedLock(lockName, timeout, cancellationToken);
    }

    private async Task<ILock> AcquireRedLock(string lockName, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        timeout ??= TimeSpan.FromSeconds(10);

        logger.LogDebug("Acquiring lock {LockName} with timeout {Timeout}", lockName, timeout.Value);
        var redLock = await redLockFactory.CreateLockAsync(lockName, TimeSpan.FromMinutes(10), timeout.Value, TimeSpan.FromMilliseconds(100), cancellationToken);

        if (!redLock.IsAcquired)
        {
            logger.LogError("Failed to acquire lock {LockName}", lockName);
            throw new AcquireLockException();
        }

        logger.LogDebug("Acquired lock {LockName}", lockName);
        var newLock = new RedLock(redLock);

        _heldLocks.Add(lockName, newLock);

        newLock.Disposed += (_, _) => {
            logger.LogDebug("Releasing lock {LockName}", lockName);
            _heldLocks.Remove(lockName);
        };

        return newLock;
    }

    private async Task<ILock> ReacquireRedLock(RedLock redLock, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
    {
        try
        {
            redLock.Reacquire();
            return redLock;
        }
        catch (LockDisposedException)
        {
            return await AcquireRedLock(redLock.LockName, timeout, cancellationToken);
        }
    }
}