
using System;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Handlers;

namespace Anderson.Pipelines.Builders
{
    /// <summary>
    /// Helps define your pipeline
    /// </summary>
    /// <typeparam name="TRoot">This is the Root node that starts the pipeline</typeparam>
    /// <typeparam name="TResponse">The response which the pipeline produces</typeparam>
    public class PipelineDefinitionBuilder<TRoot, TResponse>
    {
        private readonly Func<Type, object> _resolveService;

        /// <summary>
        /// Uses DI container to resolve pipes and dependancies by providing
        /// the function to resolve services
        /// </summary>
        /// <param name="resolveService"></param>
        /// <returns></returns>
        public PipelineDefinitionBuilder(Func<Type, object> resolveService)
        {
            _resolveService = resolveService;
        }

        /// <summary>
        /// The initial node that begins the pipeline
        /// </summary>
        /// <param name="rootDefinition"></param>
        /// <returns></returns>
        public static PipelineDefinitionBuilder<TRoot, TRoot, TResponse> StartWith(
            PipelineDefinition<TRoot, TResponse> rootDefinition)
        {
            return new PipelineDefinitionBuilder<TRoot, TRoot, TResponse>(rootDefinition)
            {
                _root = rootDefinition
            };
        }

        /// <summary>
        /// The initial node that begins the pipeline
        /// </summary>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRoot, TRoot, TResponse> StartWith<T>() where T : PipelineDefinition<TRoot, TResponse>
        {
            PipelineDefinition<TRoot, TResponse> pipe = (PipelineDefinition<TRoot, TResponse>) _resolveService(typeof(T));

            return new PipelineDefinitionBuilder<TRoot, TRoot, TResponse>(pipe, _resolveService)
            {
                _root = pipe
            };
        }
    }

    /// <summary>
    /// Allows fluent systax to construct the rest of the pipeline
    /// </summary>
    /// <typeparam name="TRoot">The root node that is at the start of the pipeline</typeparam>
    /// <typeparam name="TChild">The node next in line</typeparam>
    /// <typeparam name="TResponse">The response which the pipeline produces</typeparam>
    public class PipelineDefinitionBuilder<TRoot, TChild, TResponse>
    {
        private readonly Func<Type, object> _resolveService;
        internal IInnerHandler<TRoot, TResponse> _root;
        private readonly IInnerHandler<TChild, TResponse> _current;

        internal PipelineDefinitionBuilder(
            IInnerHandler<TRoot, TResponse> rootDefinition,
            IInnerHandler<TChild, TResponse> currentDefinition,
            Func<Type, object> resolveService)
        {
            _root = rootDefinition;
            _current = currentDefinition;
            _resolveService = resolveService;
        }

        internal PipelineDefinitionBuilder(PipelineDefinition<TChild, TResponse> currentDefinition)
        {
            _current = currentDefinition;
        }

        internal PipelineDefinitionBuilder(PipelineDefinition<TChild, TResponse> currentDefinition, Func<Type, object> resolveService) : this(currentDefinition)
        {
            _resolveService = resolveService;
        }

        /// <summary>
        /// Adds the next node into the pipeline. Pipelines execute in the order nodes are added
        /// </summary>
        /// <param name="pipe">Defines how requests are handled</param>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRoot, TChild, TResponse> ThenWith(
            PipelineDefinition<TChild, TResponse> pipe)
        {
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TChild, TResponse>(
                _root, 
                pipe,
                _resolveService);
        }

        /// <summary>
        /// Adds the next node into the pipeline. Pipelines execute in the order nodes are added
        /// </summary>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRoot, TChild, TResponse> ThenWith<T>() where T : PipelineDefinition<TChild, TResponse>
        {
            PipelineDefinition<TChild, TResponse> pipe = (PipelineDefinition<TChild, TResponse>)_resolveService(typeof(T));
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TChild, TResponse>(
                _root,
                pipe,
                _resolveService);
        }

        /// <summary>
        /// Changes the Initial request into a different type.
        /// Further nodes in the pipeline from this point will now handle <typeparam name="TMutatedRequest"></typeparam>
        /// </summary>
        /// <typeparam name="TMutatedRequest"></typeparam>
        /// <param name="pipe"></param>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRoot, TMutatedRequest, TResponse> ThenWithMutation<TMutatedRequest>(
            PipelineMutationDefinition<TChild, TMutatedRequest, TResponse> pipe)
        {
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TMutatedRequest, TResponse>(
                _root, 
                pipe,
                _resolveService);
        }

        /// <summary>
        /// Changes the Initial request into a different type.
        /// Further nodes in the pipeline from this point will now handle <typeparam name="TMutatedRequest"></typeparam>
        /// </summary>
        /// <typeparam name="TMutatedRequest"></typeparam>
        /// <typeparam name="TPipe"></typeparam>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRoot, TMutatedRequest, TResponse> ThenWithMutation<TPipe, TMutatedRequest>() 
            where TPipe : PipelineMutationDefinition<TChild, TMutatedRequest, TResponse>
        {
            PipelineMutationDefinition<TChild, TMutatedRequest, TResponse> pipe = (PipelineMutationDefinition<TChild, TMutatedRequest, TResponse>)_resolveService(typeof(TPipe));
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TMutatedRequest, TResponse>(
                _root,
                pipe,
                _resolveService);
        }

        /// <summary>
        /// returns the pipeline
        /// </summary>
        /// <returns></returns>
        public IRequestHandler<TRoot, TResponse> Build()
        {
            return _root as PipelineDefinition<TRoot, TResponse>;
        }
    }
}