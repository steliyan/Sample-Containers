using System;
using System.Threading.Tasks;
using Autofac;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;
using MassTransit.Util;
using Sample.Components;
using Sample.Contracts;

namespace Sample_Autofac
{
    static class Program
    {
        static void Main()
        {
            var container = ConfigureContainer();

            var bus = container.Resolve<IBusControl>();

            try
            {
                bus.Start();
                try
                {
                    Console.WriteLine("Bus started, type 'exit' to exit.");

                    bool running = true;
                    while (running)
                    {
                        var input = Console.ReadLine();
                        switch (input)
                        {
                            case "exit":
                            case "quit":
                                running = false;
                                break;

                            case "submit":
                                TaskUtil.Await(() => Submit(container));
                                break;
                        }
                    }
                }
                finally
                {
                    bus.Stop();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static async Task Submit(IContainer container)
        {
            IBus bus = container.Resolve<IBus>();

            var orderId = NewId.NextGuid();

            await bus.Send<SubmitOrder>(new
            {
                OrderId = orderId,
                OrderDateTime = DateTimeOffset.Now
            }, Pipe.Execute<SendContext>(sendContext => sendContext.ConversationId = sendContext.CorrelationId = orderId));
        }

        static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.AddMassTransit(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();
                cfg.AddSagaStateMachinesFromNamespaceContaining(typeof(OrderStateMachine));
                cfg.AddActivitiesFromNamespaceContaining(typeof(FooExecuteActivity));

                cfg.AddBus(BusFactory);
            });

            builder.RegisterType<PublishOrderEventActivity>();
            builder.RegisterInMemorySagaRepository();

            return builder.Build();
        }

        static IBusControl BusFactory(IComponentContext context)
        {
            return Bus.Factory.CreateUsingInMemory(cfg => cfg.ConfigureEndpoints(context));
        }
    }
}