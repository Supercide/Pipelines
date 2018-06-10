using System;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Tests.Pipelines;

namespace Pipelines.Tests
{
    public class TestHandler<TRequest> : PipelineDefinition<TRequest, TestResponse> {
        public TRequest _request;
        public DateTime _timestamp;
        public override TestResponse Handle(TRequest request)
        {
            _request = request;
            _timestamp = DateTime.UtcNow;

            if (InnerHandler != null)
            {
                return InnerHandler.Handle(request);
            }

            return new TestResponse();
        }
    }
}