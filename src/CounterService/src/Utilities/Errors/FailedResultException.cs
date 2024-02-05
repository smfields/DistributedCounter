using FluentResults;

namespace DistributedCounter.CounterService.Utilities.Errors;

public class FailedResultException(ResultBase result) : Exception
{
    public ResultBase Result { get; } = result;
}