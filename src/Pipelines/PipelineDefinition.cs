namespace Pipelines
{
    public abstract class PipelineDefinition<TRequest, TResponse> 
        : IRequestHandler<TRequest, TResponse>, 
          IInnerHandler<TRequest, TResponse>
    {
        public IRequestHandler<TRequest, TResponse> InnerHandler { get; set; }
        public abstract TResponse Handle(TRequest request);
    }
}