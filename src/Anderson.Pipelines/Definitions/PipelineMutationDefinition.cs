using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Handlers;

namespace Anderson.Pipelines.Definitions
{
    public abstract class PipelineMutationDefinition<TRequest, TMutatedRequest> 
        : IRequestHandler<TRequest>, 
          IInnerHandler<TMutatedRequest>
    {
        public IRequestHandler<TMutatedRequest> InnerHandler { get; set; }

        public abstract Task HandleAsync(TRequest request, Context context, CancellationToken token);
    }
}