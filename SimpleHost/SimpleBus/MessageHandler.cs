using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBus
{
    public abstract class MessageHandler<T>
    {
        public Bus Bus { get; set; }

        public abstract void Handle(T message);

        internal void HandleMessage(QueueMessage tMessage)
        {
            Handle((T)tMessage.Body);
        }
    }
}
