using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBus
{
    public interface IMessageQueueSystem
    {
        QueueMessage PopMessage(string queueName);
        void InsertMessage(QueueMessage message, string queueName);
        void RollbackMessagePop(QueueMessage message);

        void CreateQueueIfDoesNotExist(string queueName);
    }
}
