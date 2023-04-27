using System.Text;
using System.Text.Json;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infra.Bus
{
	public sealed class RabbitMQBus : IEventBus
	{
		private readonly IMediator _mediator;
		private readonly Dictionary<string, List<Type>> _handlers;
		private readonly List<Type> _eventTypes;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
			_mediator = mediator;
			_handlers = new Dictionary<string, List<Type>>();
			_eventTypes = new List<Type>();
		}

		public Task SendCommand<T>(T command) where T : Command
		{
			return _mediator.Send(command);
		}

		public void Publish<T>(T @event) where T : Event
		{
			var factory = new ConnectionFactory();
			factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();

			var eventName = @event.GetType().Name;

			channel.QueueDeclare(eventName, false, false, false, null);

			var message = JsonSerializer.Serialize(@event);
			var body = Encoding.UTF8.GetBytes(message);

			channel.BasicPublish(string.Empty, eventName, null, body);
		}

		public void Subscribe<T, U>()
			where T : Event
			where U : IEventHandler<T>
		{
			var eventName = typeof(T).Name;
			var handlerType = typeof(U);

			if (!_eventTypes.Contains(typeof(T)))
				_eventTypes.Add(typeof(T));

			if (!_handlers.ContainsKey(eventName))
				_handlers.Add(eventName, new List<Type>());

			if (_handlers[eventName].Any(s => s.GetType() == handlerType))
				throw new ArgumentException(
					$"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType)
				);

			_handlers[eventName].Add(handlerType);

			StartBasicConsume<T>();
		}

		private void StartBasicConsume<T>() where T : Event
		{
			var factory = new ConnectionFactory();
			factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
			factory.DispatchConsumersAsync = true;
			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();

			var eventName = typeof(T).Name;

			channel.QueueDeclare(eventName, false, false, false, null);

			var consumer = new AsyncEventingBasicConsumer(channel);
			consumer.Received += ConsumerReceived;

			channel.BasicConsume(eventName, true, consumer);

			async Task ConsumerReceived(object sender, BasicDeliverEventArgs e)
			{
				var eventName = e.RoutingKey;
				var message = Encoding.UTF8.GetString(e.Body.ToArray());

				try
				{
					await ProcessEvent(eventName, message).ConfigureAwait(false);
				}
				catch (Exception ex)
				{

				}
			}

			async Task ProcessEvent(string eventName, string message)
			{
				if (_handlers.ContainsKey(eventName))
				{
					using (var scope = _serviceScopeFactory.CreateScope())
					{
						var subscriptions = _handlers[eventName];

						foreach (var subscription in subscriptions)
						{
							var handler = scope.ServiceProvider.GetService(subscription);

							if (handler is null) continue;

							var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
							var @event = JsonSerializer.Deserialize(message, eventType!);
							var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType!);

							await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, new object[] { @event! })!;
						}
					};
				}
			}
		}
	}
}