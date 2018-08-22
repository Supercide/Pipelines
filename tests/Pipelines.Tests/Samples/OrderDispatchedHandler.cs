using System;
using System.Threading;
using System.Threading.Tasks;
using Andersoft.Pipelines.Definitions;

namespace Andersoft.Pipelines.Tests.Samples
{
    public class OrderDispatchedHandler : PipelineDefinition<Order>
    {
        public override Task HandleAsync(Order request, Context context, CancellationToken token = default(CancellationToken))
        {
            context.SetResponse(new OrderDispatched
            {
                Dispatched = DateTime.UtcNow
            });

            return Task.CompletedTask;
        }
    }
}