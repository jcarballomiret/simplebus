using System;
using System.Collections.Generic;
using Castle.Windsor;
using Newtonsoft.Json;

namespace SimpleBus
{
    public class Bus
    {
        public WindsorContainer Container { get; internal set; }
        public IMessageQueueSystem Transport { get; set; }

        public Dictionary<Type, string> Routings { get; set; }

        public Bus(WindsorContainer container)
        {
            Container = container;
            Routings = new Dictionary<Type, string>();
        }

        public string EndpointName { get; set; }


        public void SendMessage(object message)
        {
            var destinyQueue = Routings[message.GetType()];
            
            var messageJson = JsonConvert.SerializeObject(message);
           
            Transport.InsertMessage(new QueueMessage()
            {
                Id = Guid.NewGuid().ToString(),
                Type = message.GetType().ToString(),
                Body = messageJson,
                Headers = "",
                SentDateTime = DateTime.UtcNow
            }, 
            destinyQueue);

        }
    }
}
