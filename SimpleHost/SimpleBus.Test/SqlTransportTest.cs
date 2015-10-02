using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SimpleBus.SqlTransport;
using SimpleBus.Test.TestObjects;

namespace SimpleBus.Test
{
    [TestClass]
    public class SqlTransportTest
    {
        private readonly string _connectionString =
            $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Environment.CurrentDirectory}\\Data\\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30"
            .Replace("bin\\Debug\\", "");

        public SqlTransportTest()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
        }

        [TestMethod]
        public void SqlMessageQueueSystem_TestFIFO()
        {
            var queueName = "queue_" + Guid.NewGuid().ToString().Replace("-", "");

            var transport = new SqlMessageQueueSystem { TransportConnectionString = _connectionString };

            var message1 = new TestMessage { MessageText = "Message1" };

            var message2 = new TestMessage { MessageText = "Message2" };

            transport.CreateQueueIfDoesNotExist(queueName);

            transport.InsertMessage(new QueueMessage
            {
                Id = Guid.NewGuid().ToString(),
                Body = JsonConvert.SerializeObject(message1),
                SentDateTime = DateTime.Now,
                Type = message1.GetType().ToString()
            }, queueName);

            transport.InsertMessage(new QueueMessage
            {
                Id = Guid.NewGuid().ToString(),
                Body = JsonConvert.SerializeObject(message2),
                SentDateTime = DateTime.Now,
                Type = message2.GetType().ToString()
            }, queueName);

            Assert.AreEqual(message1.MessageText, ((TestMessage)transport.PopMessage(queueName).Body).MessageText);
            Assert.AreEqual(message2.MessageText, ((TestMessage)transport.PopMessage(queueName).Body).MessageText);
        }

        public void CleanUpDatabase()
        {

        }
    }
}
