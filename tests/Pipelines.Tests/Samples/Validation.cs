using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;

namespace Anderson.Pipelines.Tests.Samples
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