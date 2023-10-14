using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using DistributedCounter.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DistributedCounter.CounterService.API.Services;

public class Counter
{
    public long Value => _value;

    private long _value = 0;

    public void Increment(uint amount)
    {
        Interlocked.Add(ref _value, amount);
    }

    public void Decrement(uint amount)
    {
        Interlocked.Add(ref _value, -1 * amount);
    }
}

public class CounterService : Protos.CounterService.CounterServiceBase
{
    private static readonly ConcurrentDictionary<Guid, Counter> Counters = new();
    
    public override Task<CounterServiceMetadata> GetMetadata(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new CounterServiceMetadata
        {
            ServiceMetadata = new ServiceMetadata
            {
                Language = RuntimeInformation.FrameworkDescription,
                Version = "0.0.1",
            },
            ServerMetadata = new ServerMetadata
            {
                Id = Guid.NewGuid().ToString(),
                Platform = RuntimeInformation.RuntimeIdentifier,
                UpTime = Environment.TickCount64
            },
            DatabaseMetadata = new DatabaseMetadata
            {
                Id = Guid.NewGuid().ToString(),
                Type = "None"
            }
        });
    }

    public override Task<IncrementCounterResponse> IncrementCounter(IncrementCounterRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CounterId, out var id))
        {
            var status = new Status(
                StatusCode.InvalidArgument,
                $"{nameof(request.CounterId)} is not a valid GUID"
            );
            throw new RpcException(status);
        }

        var counter = Counters.GetOrAdd(id, new Counter());
        counter.Increment(request.IncrementAmount);

        return Task.FromResult(new IncrementCounterResponse());
    }

    public override Task<DecrementCounterResponse> DecrementCounter(DecrementCounterRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CounterId, out var id))
        {
            var status = new Status(
                StatusCode.InvalidArgument,
                $"{nameof(request.CounterId)} is not a valid GUID"
            );
            throw new RpcException(status);
        }

        var counter = Counters.GetOrAdd(id, new Counter());
        counter.Decrement(request.DecrementAmount);

        return Task.FromResult(new DecrementCounterResponse());
    }

    public override Task<GetCurrentValueResponse> GetCurrentValue(GetCurrentValueRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CounterId, out var id))
        {
            var status = new Status(
                StatusCode.InvalidArgument,
                $"{nameof(request.CounterId)} is not a valid GUID"
            );
            throw new RpcException(status);
        }

        var counter = Counters.GetOrAdd(id, new Counter());

        return Task.FromResult(new GetCurrentValueResponse
        {
            CounterId = id.ToString(),
            Value = counter.Value
        });
    }
}