using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Transfer.Domain.Events
{
	public class TransferCreatedEvent : Event
	{
		public int From { get; private set; }
		public int To { get; private set; }
		public double Amount { get; private set; }

		public TransferCreatedEvent(int from, int to, double amount)
		{
			From = from;
			To = to;
			Amount = amount;
		}
	}
}