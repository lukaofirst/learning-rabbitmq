using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var arguments = new Dictionary<string, object>() 
{
	{ "x-match", "any" },
	{ "format", "pdf" },
	{ "type", "text" }
};

channel.QueueDeclare("headers-example-consumer2", false, false, false, arguments);
channel.QueueBind("headers-example-consumer2", "HeadersExchange", string.Empty, arguments);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);

	Console.WriteLine($"Received message: {message}");

	channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume("headers-example-consumer2", false, consumer);

Console.ReadKey();
