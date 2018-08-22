using System;
using Andersoft.Pipelines.Definitions;
using Andersoft.Pipelines.Handlers;

namespace Andersoft.Pipelines.Builders
{
    /// <summary>
    /// Helps define your pipeline
    /// </summary>
    public class PipelineDefinitionBuilder
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
        public static PipelineDefinitionBuilder<TRoot, TRoot> StartWith<TRoot>(
            PipelineDefinition<TRoot> rootDefinition)
        {
            return new PipelineDefinitionBuilder<TRoot, TRoot>(rootDefinition)
            {
                _root = rootDefinition
            };
        }

        /// <summary>
        /// The initial node that begins the pipeline
        /// </summary>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRequest, TRequest> StartWith<TPipe, TRequest>() where TPipe : PipelineDefinition<TRequest>
        {
            PipelineDefinition<TRequest> pipe = (PipelineDefinition<TRequest>) _resolveService(typeof(TPipe));

            return new PipelineDefinitionBuilder<TRequest, TRequest>(pipe, _resolveService)
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
    public class PipelineDefinitionBuilder<TRoot, TChild>
    {
        private readonly Func<Type, object> _resolveService;
        internal IInnerHandler<TRoot> _root;
        private readonly IInnerHandler<TChild> _current;

        internal PipelineDefinitionBuilder(
            IInnerHandler<TRoot> rootDefinition,
            IInnerHandler<TChild> currentDefinition,
            Func<Type, object> resolveService)
        {
            _root = rootDefinition;
            _current = currentDefinition;
            _resolveService = resolveService;
        }

        internal PipelineDefinitionBuilder(PipelineDefinition<TChild> currentDefinition)
        {
            _current = currentDefinition;
        }

        internal PipelineDefinitionBuilder(PipelineDefinition<TChild> currentDefinition, Func<Type, object> resolveService) : this(currentDefinition)
        {
            _resolveService = resolveService;
        }

        /// <summary>
        /// Adds the next node into the pipeline. Pipelines execute in the order nodes are added
        /// </summary>
        /// <param name="pipe">Defines how requests are handled</param>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRoot, TChild> ThenWith(
            PipelineDefinition<TChild> pipe)
        {
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TChild>(
                _root, 
                pipe,
                _resolveService);
        }

        /// <summary>
        /// Adds the next node into the pipeline. Pipelines execute in the order nodes are added
        /// </summary>
        /// <returns></returns>
        public PipelineDefinitionBuilder<TRoot, TChild> ThenWith<T>() where T : PipelineDefinition<TChild>
        {
            PipelineDefinition<TChild> pipe = (PipelineDefinition<TChild>)_resolveService(typeof(T));
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TChild>(
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
        public PipelineDefinitionBuilder<TRoot, TMutatedRequest> ThenWithMutation<TMutatedRequest>(
            PipelineMutationDefinition<TChild, TMutatedRequest> pipe)
        {
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TMutatedRequest>(
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
        public PipelineDefinitionBuilder<TRoot, TMutatedRequest> ThenWithMutation<TPipe, TMutatedRequest>() 
            where TPipe : PipelineMutationDefinition<TChild, TMutatedRequest>
        {
            PipelineMutationDefinition<TChild, TMutatedRequest> pipe = (PipelineMutationDefinition<TChild, TMutatedRequest>)_resolveService(typeof(TPipe));
            _current.InnerHandler = pipe;
            return new PipelineDefinitionBuilder<TRoot, TMutatedRequest>(
                _root,
                pipe,
                _resolveService);
        }

        /// <summary>
        /// returns the pipeline
        /// </summary>
        /// <returns></returns>
        public IRequestHandler<TRoot> Build()
        {
            return _root as PipelineDefinition<TRoot>;
        }
    }
}