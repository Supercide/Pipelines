using System;
using System.Threading;
using System.Threading.Tasks;
using Anderson.Pipelines.Definitions;

namespace Anderson.Pipelines.Tests.Pipelines
{
    public class TestHandlerA<TRequest> : TestHandler<TRequest> { }
    public class TestHandlerB<TRequest> : TestHandler<TRequest> { }
    public class TestHandlerC<TRequest> : TestHandler<TRequest> { }

    public class TestHandler<TRequest> : PipelineDefinition<TRequest> {
        public TRequest _request;
        public DateTime _timestamp;
        public override Task HandleAsync(TRequest request, Context context, CancellationToken token = default(CancellationToken))
        {
            _request = request;
            _timestamp = DateTime.UtcNow;

            if (InnerHandler != null)
            {
                return InnerHandler.HandleAsync(request, context, token);
            }

            context.SetResponse(new TestResponse());

            return Task.CompletedTask;
        }
    }


}