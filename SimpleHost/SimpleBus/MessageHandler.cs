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
