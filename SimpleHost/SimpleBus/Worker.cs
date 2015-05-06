﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using SimpleBus.Exceptions;
using SimpleBus.Helpers;

namespace SimpleBus
{

    public class Worker
    {
        public Bus Bus { get; set; }
        public Worker(Bus bus)
        {
            Bus = bus;
            Configure(bus);
        }



        private void Configure(Bus bus)
        {
            // Injecting the Bus into all MessageHandler descendants
            var messageHandlerType = typeof(MessageHandler<>);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => TypeHelper.IsTypeDerivedFromGenericType(p, messageHandlerType));

            foreach (var type in types)
            {
                bus.Container.Register(Component.For(type).DependsOn(
                    Property.ForKey("Bus").Eq(bus)
                    ));
            }

            // Creating the queues
            Bus.Transport.CreateQueueIfDoesNotExist(Bus.EndpointName);
            foreach (var routing in Bus.Routings)
            {
                Bus.Transport.CreateQueueIfDoesNotExist(routing.Value);
            }
        }

        public void Start()
        {
            WhileHelper.While(
                new ParallelOptions()
            {
                MaxDegreeOfParallelism = 5
            },
            () => true,
                f =>
                {
                    var message = Bus.Transport.PopMessage(Bus.EndpointName);

                    if (message == null)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        var messageType = TypeHelper.GetType(message.Type);
                        var handlerType = GetHandlerForMessageType(messageType); 

                        var handler = Activator.CreateInstance(handlerType);
                        MethodInfo handlerMessageMethodInfo = handlerType.GetMethod("HandleMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                        try
                        {
                            handlerMessageMethodInfo.Invoke(handler, new object[] { message });
                        }
                        catch (Exception ex)
                        {
                            Bus.Transport.RollbackMessagePop(message);
                        }
                    }

                });

        }

        public Type GetHandlerForMessageType(Type mType)
        {
            var specificMessageHandlerType = typeof(MessageHandler<>).MakeGenericType(mType);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsSubclassOf(specificMessageHandlerType)).ToList();

            if (types.Count == 0)
                throw new HandlerNotFoundForMessage(mType);

            return types.First();
        }
    }
}
