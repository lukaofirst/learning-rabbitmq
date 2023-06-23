using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("TopicExchange", ExchangeType.Topic);

/* Tip: The topic-example-queue will store all the messages with routingKey
[countries.*] without this queue, the messages will be lost */
channel.QueueDeclare("topic-example-queue", false, false, false, null);
channel.QueueBind("topic-example-queue", "TopicExchange", "countries.#");

var countries = new string[] { "Brazil", "EUA", "Belgium", "Canada", "England" };

Console.WriteLine("How many message do you want to create?");
var times = int.Parse(Console.ReadLine()!);
var rnd = new Random();

for (int i = 0; i < times; i++)
{
	var randomCountry = countries[rnd.Next(0, countries.Length)];
	var routingKey = $"countries.{randomCountry}.location";

	var message = Encoding.UTF8.GetBytes($"Message from [{randomCountry}]");

	channel.BasicPublish("TopicExchange", routingKey, null, message);
}
