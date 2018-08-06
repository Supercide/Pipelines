using System;
using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;

namespace Anderson.Pipelines.Tests.Samples
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