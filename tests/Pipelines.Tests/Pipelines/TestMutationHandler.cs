using System;
using System.Threading;
using System.Threading.Tasks;
using Andersoft.Pipelines.Definitions;

namespace Andersoft.Pipelines.Tests.Pipelines
{
    public class TestMutationHandler : PipelineMutationDefinition<TestRequestA, TestRequestB>
    {
        public TestRequestA _request;
        public DateTime _timestamp;
        public override Task HandleAsync(TestRequestA request, Context context, CancellationToken token = default(CancellationToken))
        {
            _request = request;
            _timestamp = DateTime.UtcNow;

            if (InnerHandler != null)
            {
                return InnerHandler.HandleAsync(new TestRequestB(), new Context(), token);
            }
            context.SetResponse(new TestResponse());
            return Task.CompletedTask;
        }
    }
}
