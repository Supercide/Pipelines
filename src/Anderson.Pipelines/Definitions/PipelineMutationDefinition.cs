using System.Threading;
using System.Threading.Tasks;
using Andersoft.Pipelines.Handlers;

namespace Andersoft.Pipelines.Definitions
{
    public abstract class PipelineMutationDefinition<TRequest, TMutatedRequest> 
        : IRequestHandler<TRequest>, 
          IInnerHandler<TMutatedRequest>
    {
        public IRequestHandler<TMutatedRequest> InnerHandler { get; set; }

        public abstract Task HandleAsync(TRequest request, Context context, CancellationToken token);
    }
}