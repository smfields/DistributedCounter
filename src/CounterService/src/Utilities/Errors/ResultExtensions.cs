using FluentResults;

namespace DistributedCounter.CounterService.Utilities.Errors;

public static class ResultExtensions
{
    public static IError? GetPrimaryError(this ResultBase result)
    {
        return result.Errors.FirstOrDefault();
    }
    
    public static void ThrowIfFailed(this ResultBase result)
    {
        if (result.IsFailed)
        {
            throw new FailedResultException(result);
        }
    }
}