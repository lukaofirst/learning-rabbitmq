using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus
{
	public interface IEventHandler<T> : IEventHandler
		where T : Event
	{
		Task Handle(T @event);
	}

	public interface IEventHandler { }
}