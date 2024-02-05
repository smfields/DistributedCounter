namespace DistributedCounter.CounterService.Utilities.Errors;

public interface IErrorVisitor
{
    public void Visit<TError>(TError error) where TError : DetailedError;
}