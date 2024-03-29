using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repositories
{
	public class TransferRepository : ITransferRepository
	{
		private readonly TransferDbContext _context;
		public TransferRepository(TransferDbContext context)
		{
			_context = context;
		}

		public IEnumerable<TransferLog> GetTransferLogs()
		{
			return _context.TransferLogs!;
		}

		public void AddTransferLog(TransferLog transferLog)
		{
			_context.Add(transferLog);
			_context.SaveChanges();
		}
	}
}