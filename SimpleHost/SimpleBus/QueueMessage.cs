using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBus
{
    public class QueueMessage
    {
        public string Id { get; set; }
        public string OriginalQueue { get; set; }
        public DateTime SentDateTime { get; set; }
        public string Type { get; set; } 
        public object Body { get; set; }
        public object Headers { get; set; }
    }
}
