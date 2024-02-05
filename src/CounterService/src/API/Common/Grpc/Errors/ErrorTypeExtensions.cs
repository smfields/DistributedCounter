using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public static class ErrorTypeExtensions
{
    public static int ToCode(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Cancelled => 1,
            ErrorType.Unknown => 2,
            ErrorType.InvalidArgument => 3,
            ErrorType.DeadlineExceeded => 4,
            ErrorType.NotFound => 5,
            ErrorType.AlreadyExists => 6,
            ErrorType.PermissionDenied => 7,
            ErrorType.ResourceExhausted => 8,
            ErrorType.FailedPrecondition => 9,
            ErrorType.Aborted => 10,
            ErrorType.OutOfRange => 11,
            ErrorType.Unimplemented => 12,
            ErrorType.Internal => 13,
            ErrorType.Unavailable => 14,
            ErrorType.DataLoss => 15,
            ErrorType.Unauthenticated => 16,
            _ => 2 // Unknown
        };
    }
}