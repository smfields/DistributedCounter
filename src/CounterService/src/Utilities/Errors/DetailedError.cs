using FluentResults;

namespace DistributedCounter.CounterService.Utilities.Errors;

public abstract class DetailedError : Error
{
    public abstract ErrorType Type { get; }
    public abstract string Identifier { get; }
    
    protected DetailedError()
    {
    }

    protected DetailedError(string message) : base(message)
    {
    }

    public abstract void Accept(IErrorVisitor visitor);
}