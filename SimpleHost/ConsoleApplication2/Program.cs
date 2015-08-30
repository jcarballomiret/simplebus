using System;
using System.Collections.Generic;
using System.Configuration;
using Castle.Windsor;
using SimpleBus;
using SimpleBus.SqlTransport;

namespace ConsoleApplication2
{
    class Program
    {     
        static void Main(string[] args)
        {
            var bus = new Bus(new WindsorContainer())
            {
                EndpointName = "Test_MyMessageQueue",
                Transport = new SqlMessageQueueSystem()
                {
                    TransportConnectionString = ConfigurationManager.ConnectionStrings["TransportDB"].ConnectionString
                },
                Routings = new Dictionary<Type, string>()
                {
                    {typeof(MyMessage), "Test_MyMessageQueue"}
                }
            };

           

            var worker = new Worker(bus);
            // Sending messages to be processed.
            for (int i = 0; i < 10000; i++)
                bus.SendMessage(new MyMessage() { Message = i.ToString() });

            // Starting the worker, this will continue monitoring the "Test_MyMessageQueue" queue.
            worker.Start();
        }
    }

    public class MyMessage
    {
        public string Message { get; set; }
    }

    public class MyMessagesHandler : MessageHandler<MyMessage>
    {        
        public override void Handle(MyMessage message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}
