using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) => 
{
	var body = ea.Body.ToArray();
	var receivedMessage = Encoding.UTF8.GetString(body);

	Console.WriteLine($"Received Message: {receivedMessage}");

	channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume("example-queue", false, consumer);

Console.ReadKey();