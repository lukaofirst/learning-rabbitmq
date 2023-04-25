using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Data.Context;
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

			// Application Services

			// Data
			services.AddTransient<TransferDbContext>();
		}
	}
}