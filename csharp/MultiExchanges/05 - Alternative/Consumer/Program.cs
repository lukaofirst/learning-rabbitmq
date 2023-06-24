using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var arguments = new Dictionary<string, object>() 
{
	{ "alternate-exchange", "AlternateExchange" }
};

channel.ExchangeDeclare("DExchange", ExchangeType.Direct, arguments: arguments);
channel.ExchangeDeclare("AlternateExchange", ExchangeType.Fanout);

channel.QueueDeclare("example-queue", false, false, false, null);
channel.QueueBind("example-queue", "AlternateExchange", string.Empty);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);

	Console.WriteLine($"Received message: {message}");

	channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume("example-queue", false, consumer);

Console.ReadKey();