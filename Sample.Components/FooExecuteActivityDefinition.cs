using System;
using MassTransit.Definition;

namespace Sample.Components
{
    public class FooExecuteActivityDefinition : ExecuteActivityDefinition<FooExecuteActivity, FooArgs>
    {
        public FooExecuteActivityDefinition()
        {
            this.ExecuteEndpointName = "FOO";
            Console.Out.WriteLine("FooExecuteActivityDefinition ctor");
        }
    }
}
