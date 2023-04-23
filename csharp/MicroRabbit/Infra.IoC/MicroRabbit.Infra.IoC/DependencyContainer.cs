using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repositories;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC
{
	public static class DependencyContainer
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			// Domain Bus
			services.AddTransient<IEventBus, RabbitMQBus>();

			// Application Services
			services.AddTransient<IAccountService, AccountService>();

			// Data
			services.AddTransient<IAccountRepository, AccountRepository>();
			services.AddTransient<BankingDbContext>();
		}
	}
}