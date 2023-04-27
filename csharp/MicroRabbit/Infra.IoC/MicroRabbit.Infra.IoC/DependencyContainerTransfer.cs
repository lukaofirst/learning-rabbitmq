using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repositories;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC
{
	public static class DependencyContainerTransfer
	{
		public static void RegisterTransferServices(this IServiceCollection services)
		{
			// Domain Bus
			services.AddTransient<IEventBus, RabbitMQBus>(sp =>
			{
				var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
				return new RabbitMQBus(sp.GetService<IMediator>()!, scopeFactory);
			});


			services.AddTransient<TransferEventHandler>();



			// Domain Transfer Commands
			services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

			// Application Services
			services.AddTransient<ITransferService, TransferService>();

			// Data
			services.AddTransient<ITransferRepository, TransferRepository>();
			services.AddTransient<TransferDbContext>();

			using var scope = services.BuildServiceProvider().CreateScope();

			var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

			eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
		}
	}
}