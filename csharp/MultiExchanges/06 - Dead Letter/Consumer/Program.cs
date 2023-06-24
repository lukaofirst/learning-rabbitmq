using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("dead-letter-queue", false, false, false, null);
channel.QueueBind("dead-letter-queue", "DeadLetterExchange", "dead-letter-routing-key");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);

	Console.WriteLine($"Received message: {message}");

	channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume("dead-letter-queue", false, consumer);

Console.ReadKey();