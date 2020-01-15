using System;
using MassTransit.Definition;

namespace Sample.Components
{
    public class FooConsumerDefinition : ConsumerDefinition<FooConsumer>
    {
        public FooConsumerDefinition()
        {
            this.EndpointName = "FOO";
            Console.Out.WriteLine("FooConsumerDefinition ctor");
        }
    }
}
