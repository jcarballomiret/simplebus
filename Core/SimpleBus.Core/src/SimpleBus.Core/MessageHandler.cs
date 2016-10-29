using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBus.Core
{
    public abstract class MessageHandler<T>
    {
        public SimpleBus Bus { get; set; }

        public abstract void Handle(T message);

        internal void HandleMessage(QueueMessage tMessage)
        {
            Handle((T)tMessage.Body);
        }
    }
}
