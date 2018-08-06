using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;

namespace Anderson.Pipelines.Handlers
{
    public interface IRequestHandler<in TRequest>
    {
        Task HandleAsync(TRequest request, Context context, CancellationToken token = default(CancellationToken));
    }
}