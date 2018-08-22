using System.Threading;
using System.Threading.Tasks;
using Andersoft.Pipelines.Definitions;

namespace Andersoft.Pipelines.Tests.Samples
{
    public class Validation : PipelineDefinition<Order>
    {
        public override Task HandleAsync(Order request, Context context, CancellationToken token)
        {
            if (string.IsNullOrEmpty(request.Address))
            {
                context.SetError(OrderError.Validation);

                return Task.CompletedTask;
            }

            return InnerHandler.HandleAsync(request, context, token);
        }
    }
}