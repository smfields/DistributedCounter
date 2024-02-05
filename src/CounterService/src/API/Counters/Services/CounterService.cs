extern alias commonprotos;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DistributedCounter.CounterService.API.Common.Grpc.Errors;
using DistributedCounter.CounterService.Application.Counters;
using DistributedCounter.CounterService.Utilities.Errors;
using DistributedCounter.Protos;
using FluentResults;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using CreateCounterResponse = DistributedCounter.Protos.CreateCounterResponse;
using DecrementCounterResponse = DistributedCounter.Protos.DecrementCounterResponse;
using IncrementCounterResponse = DistributedCounter.Protos.IncrementCounterResponse;

namespace DistributedCounter.CounterService.API.Counters.Services;

public class CounterService(ISender sender) : Protos.CounterService.CounterServiceBase
{
    private static readonly Guid ServerId = Guid.NewGuid();

    public override Task<CounterServiceMetadata> GetMetadata(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new CounterServiceMetadata
        {
            ServiceMetadata = new ServiceMetadata
            {
                Language = RuntimeInformation.FrameworkDescription,
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            },
            ServerMetadata = new ServerMetadata
            {
                Id = ServerId.ToString(),
                Platform = RuntimeInformation.RuntimeIdentifier,
                UpTime = Environment.TickCount64
            },
            DatabaseMetadata = new DatabaseMetadata
            {
                Id = Guid.Empty.ToString(),
                Type = "None"
            }
        });
    }

    public override async Task<CreateCounterResponse> CreateCounter(CreateCounterRequest request, ServerCallContext context)
    {
        var command = new CreateCounter.Command(request.InitialValue);
        var response = await sender.Send(command);
        
        return new CreateCounterResponse
        {
            CounterId = response.CounterId.ToString()
        };
    }

    public override async Task<ResetCounterResponse> ResetCounter(ResetCounterRequest request, ServerCallContext context)
    {
        var id = ParseGuid(request.CounterId, "counter_id");
        var command = new ResetCounter.Command(id, request.UpdatedValue);
        await sender.Send(command);

        return new ResetCounterResponse();
    }

    public override async Task<IncrementCounterResponse> IncrementCounter(IncrementCounterRequest request, ServerCallContext context)
    {
        var id = ParseGuid(request.CounterId, "counter_id");
        var command = new IncrementCounter.Command(id, request.IncrementAmount);
        var response = await sender.Send(command);
        
        response.Result.ThrowIfFailed();

        return new IncrementCounterResponse();
    }

    public override async Task<DecrementCounterResponse> DecrementCounter(DecrementCounterRequest request, ServerCallContext context)
    {
        var id = ParseGuid(request.CounterId, "counter_id");
        var command = new DecrementCounter.Command(id, request.DecrementAmount);
        await sender.Send(command);

        return new DecrementCounterResponse();
    }

    public override async Task<GetCurrentValueResponse> GetCurrentValue(GetCurrentValueRequest request, ServerCallContext context)
    {
        var id = ParseGuid(request.CounterId, "counter_id");
        var query = new GetCounterValue.Query(id);
        var response = await sender.Send(query);

        return new GetCurrentValueResponse
        {
            CounterId = request.CounterId,
            Value = response.CurrentValue
        };
    }

    private Guid ParseGuid(string value, string fieldPath)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return guid;
        }
        
        Result result = new InvalidArgumentError(
        [
            new InvalidArgumentError.FieldViolation(fieldPath, $"Could not parse \"{value}\" as a GUID")
        ]);
        throw new FailedResultException(result);
    }
}