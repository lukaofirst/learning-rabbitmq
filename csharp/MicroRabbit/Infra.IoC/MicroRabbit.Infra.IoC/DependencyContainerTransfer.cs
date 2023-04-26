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
			services.AddTransient<IEventBus, RabbitMQBus>();

			// Domain Transfer Commands
			services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

			// Application Services
			services.AddTransient<ITransferService, TransferService>();

			// Data
			services.AddTransient<ITransferRepository, TransferRepository>();
			services.AddTransient<TransferDbContext>();
		}
	}
}