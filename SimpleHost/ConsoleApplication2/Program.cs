using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SimpleBus;
using SimpleBus.SqlTransport;

namespace ConsoleApplication2
{
    class Program
    {
        private static void SetupCategory(string categoryName)
        {
            if (!PerformanceCounterCategory.Exists(categoryName))
            {

                CounterCreationDataCollection CCDC = new CounterCreationDataCollection();

                // Add the counter.
                CounterCreationData averageCount64 = new CounterCreationData();
                averageCount64.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
                averageCount64.CounterName = "RateMessagesPerSecond";
                CCDC.Add(averageCount64);

                PerformanceCounterCategory.Create(categoryName, "Message per second rate.", CCDC);

                return;
            }
            Console.WriteLine("Category exists - AverageCounter64SampleCategory");
        }

        static void Main(string[] args)
        {
            SetupCategory("SimpleBus1");


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

            for (int i = 0; i < 10000; i++)
                bus.SendMessage(new MyMessage() { Message = i.ToString() });

            worker.Start();

            Console.ReadLine();
        }
    }

    public class MyMessage
    {
        public string Message { get; set; }
    }

    public class MyMessagesHandler : MessageHandler<MyMessage>
    {
        static PerformanceCounter pc = new PerformanceCounter("SimpleBus1", "RateMessagesPerSecond",false);
        public override void Handle(MyMessage message)
        {
            pc.Increment();
            Console.WriteLine(message.ToString());
        }
    }
}
