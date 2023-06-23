using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

/* Tip: topic-example-queue-consumer1 is only interested in messages with routingKey 
[countries.Brazil] */
channel.QueueDeclare("topic-example-queue-consumer1", false, false, false, null);
channel.QueueBind("topic-example-queue-consumer1", "TopicExchange", "countries.Brazil");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) => 
{
	var body = ea.Body.ToArray();
	var message = Encoding.UTF8.GetString(body);

	Console.WriteLine($"Received Message - Consumer1 - {message}");

	channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume("topic-example-queue-consumer1", false, consumer);

Console.ReadKey();