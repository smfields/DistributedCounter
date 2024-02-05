using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public class InvalidArgumentError(IEnumerable<InvalidArgumentError.FieldViolation> fieldViolations) : DetailedError("Invalid argument")
{
    private readonly List<FieldViolation> _fieldViolations = fieldViolations.ToList();
    
    public override ErrorType Type => ErrorType.InvalidArgument;
    public override string Identifier => "INVALID_ARGUMENT";

    public IReadOnlyList<FieldViolation> FieldViolations => _fieldViolations.AsReadOnly();

    public override void Accept(IErrorVisitor visitor)
    {
        visitor.Visit(this);
    }

    public record FieldViolation(string Field, string Description);
}