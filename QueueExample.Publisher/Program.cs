using RabbitMQ.Client;
using System;
using System.Net.WebSockets;
using System.Text;

namespace QueueExample.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            // Work with cloudamqp RabbitMQ services
            factory.Uri = new Uri("amqp://mctxyfvu:Iqmjc2CrE6O2RT0v1aUAqGLnFgJwVBbq@wasp.rmq.cloudamqp.com/mctxyfvu");

            // Work on local RabbitMQ services
            //factory.HostName = "localhost";

            var qName = "My_Tasks";
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //durable:true; kuyruğu diske yazar
                    channel.QueueDeclare(qName, durable: true, false, false, null);

                    string message = GetMessage(args);

                    for (int i = 1; i <= 10; i++)
                    {
                        var body = Encoding.UTF8.GetBytes($"{message}-{i}");

                        var prop = channel.CreateBasicProperties();
                        prop.Persistent = true;

                        channel.BasicPublish("", routingKey: qName, prop, body);
                        Console.WriteLine($"Message Sent! : {message}-{i}");
                    }
                }

                Console.ReadLine();
            }

        }

        private static string GetMessage(string[] args)
        {
            return args[0];
        }
    }
}


