using DistributedCounter.CounterService.API.GRPC.Services;
using DistributedCounter.CounterService.Application.Common;
using DistributedCounter.CounterService.Application.Counters.CreateCounter;
using DistributedCounter.CounterService.Domain.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddMediatR(opts =>
{
    opts.RegisterServicesFromAssemblyContaining<CreateCounterCommand>();
});
builder.Services.AddDbContext<ApplicationDbContext>(opts => 
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Database"))
);
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CounterService>();
app.MapGrpcReflectionService();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

}
app.Run();
