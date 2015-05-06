using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBus.Exceptions
{
    public class HandlerNotFoundForMessage : SimpleBusException
    {
        public HandlerNotFoundForMessage(Type t) { }
    }
}
