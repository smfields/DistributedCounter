extern alias commonprotos;
using DistributedCounter.CounterService.Utilities.Errors;
using Status = commonprotos::Google.Rpc.Status;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public interface IErrorStatusVisitor : IErrorVisitor
{
    public Status Status { get; }
}

public class ErrorStatusVisitor(IServiceProvider serviceProvider) : IErrorStatusVisitor
{
    private Status? _status;

    public Status Status
    {
        get
        {
            if (_status is null)
            {
                throw new InvalidOperationException("Must call Visit before attempting to retrieve status");
            }

            return _status;
        }
        private set => _status = value;
    }

    public void Visit<TError>(TError error) where TError : DetailedError
    {
        var errorConverter = serviceProvider.GetService<IErrorStatusConverter<TError>>() ?? serviceProvider.GetRequiredService<IErrorStatusConverter<DetailedError>>();
        Status = errorConverter.Convert(error);
    }
}