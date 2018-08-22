using System.Threading;
using System.Threading.Tasks;
using Andersoft.Pipelines.Handlers;

namespace Andersoft.Pipelines.Definitions
{
    public abstract class PipelineDefinition<TRequest> 
        : IRequestHandler<TRequest>, 
          IInnerHandler<TRequest>
    {
        public IRequestHandler<TRequest> InnerHandler { get; set; }
        public abstract Task HandleAsync(TRequest request, Context context, CancellationToken token = default(CancellationToken));
    }
}