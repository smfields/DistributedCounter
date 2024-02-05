extern alias commonprotos;
using DistributedCounter.CounterService.Utilities.Errors;

namespace DistributedCounter.CounterService.API.Common.Grpc.Errors;

public class DefaultErrorStatusConverter : BaseErrorStatusConverter<DetailedError>;