using System.Threading.Tasks;
using MassTransit.Courier;

namespace Sample.Components
{
    public class FooExecuteActivity : ExecuteActivity<FooArgs>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<FooArgs> context)
        {
            return context.Completed();
        }
    }
}
