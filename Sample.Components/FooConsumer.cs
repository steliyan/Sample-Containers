using System.Threading.Tasks;
using MassTransit;

namespace Sample.Components
{
    public class FooConsumer : IConsumer<FooMessage>
    {
        public Task Consume(ConsumeContext<FooMessage> context)
        {
            return context.ConsumeCompleted;
        }
    }
}
