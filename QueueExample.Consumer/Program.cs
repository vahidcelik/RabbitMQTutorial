using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace QueueExample.Consumer
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
                    channel.QueueDeclare(qName, durable: true, exclusive: false, autoDelete: false, null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, false);
                    Console.WriteLine("mesajları beliyorum....");

                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(qName, autoAck: false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine("Mesaj alındı:" + message);
                        Thread.Sleep(GetMessage(args));
                        Console.WriteLine("İşlendi");

                        channel.BasicAck(ea.DeliveryTag, multiple: false);
                    };
                }
            }
        }

        private static int GetMessage(string[] args)
        {
            return int.Parse(args[0].ToString());
        }
    }


}
