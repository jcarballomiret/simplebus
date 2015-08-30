# SimpleBus

SimpleBus is a very simple framework that facilitates the implementation of a service-oriented architecture (SOA).

By default will use SQL Server as the transport of the messages but extensible to any other transport of your convenience.

## Sample
The full code of this example is included in this repository.

Let's imagine that we have a message called **MyMessage** that will contain only a single string property.

```c#
public class MyMessage
{
    public string Message { get; set; }
}
```

For processing this type of message we need to create a class that will __handle__ it. For the simplicity of this sample, the handling of the message will only consist in writing to the console the string value if its only property. We will intruct **SimpleBus** that the class **MyMessagesHandler** will take care of all the messages of type **MyMessage** by inheriting from the class **MessageHandler<MyMessage>**.

```c#
public class MyMessagesHandler : MessageHandler<MyMessage>
{
    public override void Handle(MyMessage message)
    {
        Console.WriteLine(message.ToString());
    }
}
```
Now in a ConsoleApplication let's send and receive some messages to demostrate how to configure de Bus. For that we need to:

1. Define the **EndpointName**, this is the name of the queue where we are going to be sending and reading from the messages.

2. Set and configure the **Transport** we are going to use, we will use SQL in this case but it can be any class that implements the interface **IMessageQueueSystem**.

3. Configure the **Routings**, here we define the destination queue for each type of message.

```c#
static void Main(string[] args)
{
    var bus = new Bus(new WindsorContainer())
    {
        EndpointName = "Test_MyMessageQueue",
        Transport = new SqlMessageQueueSystem()
        {
            TransportConnectionString = ConfigurationManager.ConnectionStrings["TransportDB"].ConnectionString
        },
        Routings = new Dictionary<Type, string>()
        {
            {typeof(MyMessage), "Test_MyMessageQueue"}
        }
    };

    var worker = new Worker(bus);

    // Sending messages to be processed.
    for (int i = 0; i < 10000; i++)
        bus.SendMessage(new MyMessage() { Message = i.ToString() });

    // Starting the worker, this will continue monitoring the "Test_MyMessageQueue" queue.
    worker.Start();

}
```

## Tech
* SQL Server
* CastleWindsor
