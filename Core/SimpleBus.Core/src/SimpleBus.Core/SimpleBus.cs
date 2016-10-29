using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimpleBus.Core
{
    public class SimpleBus
    {
        public IMessageQueueSystem MessageQueueSystem { get; set; }
        public Dictionary<Type, string> Routings { get; set; }

        public SimpleBus()
        {
            Routings = new Dictionary<Type, string>();
        }

        public string EndpointName { get; set; }


        public void SendMessage(object message)
        {
            var destinyQueue = Routings[message.GetType()];

            var messageJson = JsonConvert.SerializeObject(message);

            MessageQueueSystem.InsertMessage(new QueueMessage()
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
