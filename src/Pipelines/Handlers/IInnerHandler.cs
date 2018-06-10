namespace Pipelines
{
    public interface IInnerHandler<TRequest, TResponse>
    {
        IRequestHandler<TRequest, TResponse> InnerHandler { get; set; }
    }
}