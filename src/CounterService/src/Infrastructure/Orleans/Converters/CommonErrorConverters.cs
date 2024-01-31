using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.Infrastructure.Orleans.Converters;

[RegisterConverter]
public sealed class CancelledErrorConverter : GenericErrorSurrogateConverter<CancelledError>
{
    protected override CancelledError CreateError(ReasonSurrogate surrogate)
    {
        return new CancelledError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class UnknownErrorConverter : GenericErrorSurrogateConverter<UnknownError>
{
    protected override UnknownError CreateError(ReasonSurrogate surrogate)
    {
        return new UnknownError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class InvalidArgumentErrorConverter : GenericErrorSurrogateConverter<InvalidArgumentError>
{
    protected override InvalidArgumentError CreateError(ReasonSurrogate surrogate)
    {
        return new InvalidArgumentError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class DeadlineExceededErrorConverter : GenericErrorSurrogateConverter<DeadlineExceededError>
{
    protected override DeadlineExceededError CreateError(ReasonSurrogate surrogate)
    {
        return new DeadlineExceededError(surrogate.Message);
    }
}


[RegisterConverter]
public sealed class NotFoundErrorConverter : GenericErrorSurrogateConverter<NotFoundError>
{
    protected override NotFoundError CreateError(ReasonSurrogate surrogate)
    {
        return new NotFoundError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class AlreadyExistsErrorConverter : GenericErrorSurrogateConverter<AlreadyExistsError>
{
    protected override AlreadyExistsError CreateError(ReasonSurrogate surrogate)
    {
        return new AlreadyExistsError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class PermissionDeniedErrorConverter : GenericErrorSurrogateConverter<PermissionDeniedError>
{
    protected override PermissionDeniedError CreateError(ReasonSurrogate surrogate)
    {
        return new PermissionDeniedError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class ResourceExhaustedErrorConverter : GenericErrorSurrogateConverter<ResourceExhaustedError>
{
    protected override ResourceExhaustedError CreateError(ReasonSurrogate surrogate)
    {
        return new ResourceExhaustedError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class FailedPreconditionErrorConverter : GenericErrorSurrogateConverter<FailedPreconditionError>
{
    protected override FailedPreconditionError CreateError(ReasonSurrogate surrogate)
    {
        return new FailedPreconditionError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class AbortedErrorConverter : GenericErrorSurrogateConverter<AbortedError>
{
    protected override AbortedError CreateError(ReasonSurrogate surrogate)
    {
        return new AbortedError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class OutOfRangeErrorConverter : GenericErrorSurrogateConverter<OutOfRangeError>
{
    protected override OutOfRangeError CreateError(ReasonSurrogate surrogate)
    {
        return new OutOfRangeError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class UnimplementedErrorConverter : GenericErrorSurrogateConverter<UnimplementedError>
{
    protected override UnimplementedError CreateError(ReasonSurrogate surrogate)
    {
        return new UnimplementedError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class InternalErrorConverter : GenericErrorSurrogateConverter<InternalError>
{
    protected override InternalError CreateError(ReasonSurrogate surrogate)
    {
        return new InternalError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class UnavailableErrorConverter : GenericErrorSurrogateConverter<UnavailableError>
{
    protected override UnavailableError CreateError(ReasonSurrogate surrogate)
    {
        return new UnavailableError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class DataLossErrorConverter : GenericErrorSurrogateConverter<DataLossError>
{
    protected override DataLossError CreateError(ReasonSurrogate surrogate)
    {
        return new DataLossError(surrogate.Message);
    }
}

[RegisterConverter]
public sealed class UnauthenticatedErrorConverter : GenericErrorSurrogateConverter<UnauthenticatedError>
{
    protected override UnauthenticatedError CreateError(ReasonSurrogate surrogate)
    {
        return new UnauthenticatedError(surrogate.Message);
    }
}
