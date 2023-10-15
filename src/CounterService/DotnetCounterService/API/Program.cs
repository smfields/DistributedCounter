using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Application.Counters.CreateCounter;
using DistributedCounter.CounterService.Domain.CounterAggregate;
using DistributedCounter.CounterService.Domain.Counters;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddMediatR(opts =>
{
    opts.RegisterServicesFromAssemblyContaining<CreateCounterCommand>();
});
builder.Services.AddSingleton<ICounterRepository, CounterRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

app.Run();
