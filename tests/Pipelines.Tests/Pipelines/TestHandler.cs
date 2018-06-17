using System;
using Anderson.Pipelines.Definitions;

namespace Anderson.Pipelines.Tests.Pipelines
{
    public class TestHandlerA<TRequest> : TestHandler<TRequest> { }
    public class TestHandlerB<TRequest> : TestHandler<TRequest> { }
    public class TestHandlerC<TRequest> : TestHandler<TRequest> { }

    public class TestHandler<TRequest> : PipelineDefinition<TRequest, TestResponse> {
        public TRequest _request;
        public DateTime _timestamp;
        public override TestResponse Handle(TRequest request)
        {
            _request = request;
            _timestamp = DateTime.UtcNow;

            if (InnerHandler != null)
            {
                InnerHandler.Handle(request);
            }

            return new TestResponse();
        }
    }


}