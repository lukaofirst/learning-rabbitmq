using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using static System.Console;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

using var conn = factory.CreateConnection();
using var channel = conn.CreateModel();

channel.QueueDeclare("HelloRabbitQueue", false, false, false);
channel.BasicQos(0, 1, false);

WriteLine("Getting messages from queue");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);

	WriteLine($"Result: {message}");
};

channel.BasicConsume("HelloRabbitQueue", true, consumer);

ReadLine();