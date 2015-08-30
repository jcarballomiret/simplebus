using System;

namespace SimpleBus.Exceptions
{
    public class HandlerNotFoundForMessage : SimpleBusException
    {
        public HandlerNotFoundForMessage(Type t) { }
    }
}
