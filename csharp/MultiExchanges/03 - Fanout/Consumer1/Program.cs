using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("queue-consumer1", false, false, false, null);
channel.QueueBind("queue-consumer1", "FanoutExchange", string.Empty);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) => 
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);

	Console.WriteLine($"Received message: {message}");

	channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume("queue-consumer1", false, consumer);

Console.ReadKey();