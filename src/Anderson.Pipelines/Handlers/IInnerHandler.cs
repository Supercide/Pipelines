namespace Anderson.Pipelines.Handlers
{
    public interface IInnerHandler<TRequest>
    {
        IRequestHandler<TRequest> InnerHandler { get; set; }
    }
}