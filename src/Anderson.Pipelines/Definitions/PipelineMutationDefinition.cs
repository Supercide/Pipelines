using Anderson.Pipelines.Handlers;

namespace Anderson.Pipelines.Definitions
{
    public abstract class PipelineMutationDefinition<TRequest, TMutatedRequest, TResponse> 
        : IRequestHandler<TRequest, TResponse>, 
          IInnerHandler<TMutatedRequest, TResponse>
    {
        public IRequestHandler<TMutatedRequest, TResponse> InnerHandler { get; set; }

        public abstract TResponse Handle(TRequest request);
    }
}