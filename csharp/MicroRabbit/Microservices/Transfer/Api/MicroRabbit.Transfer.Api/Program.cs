using MediatR;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.EventHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TransferDbContext>(opt =>
{
	var connStr = builder.Configuration.GetConnectionString("TransferDbConn");
	opt.UseSqlServer(connStr);
});
builder.Services.RegisterTransferServices();

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
	x.SwaggerEndpoint("/swagger/v1/swagger.json", "Example App V1");
	x.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ConfigureEventBus(app);

void ConfigureEventBus(WebApplication app)
{
	var eventBus = app.Services.GetRequiredService<IEventBus>();

	eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
}

app.Run();
