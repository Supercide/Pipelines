using System.Linq;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Tests.Pipelines;
using NUnit.Framework;

namespace Pipelines.Tests
{
    public class PipelineTest
    {
        [Test]
        public void GivenPiplelineWithThreeStages_WhenHandlingRequest_ThenAllThreeHandlersHandleRequest()
        {
            var testHandlerA = new TestHandler<TestRequestA>();
            var testHandlerB = new TestHandler<TestRequestA>();
            var testHandlerC = new TestHandler<TestRequestA>();
            
            var pipeline = PipelineDefinitionBuilder<TestRequestA, TestResponse>
                .StartWith(testHandlerA)
                .ThenWith(testHandlerB)
                .ThenWith(testHandlerC)
                .Build();

            var testRequest = new TestRequestA();

            pipeline.Handle(testRequest);
            Assert.Multiple(() =>
            {
                Assert.That(testHandlerA._request, Is.EqualTo(testRequest));
                Assert.That(testHandlerB._request, Is.EqualTo(testRequest));
                Assert.That(testHandlerC._request, Is.EqualTo(testRequest));
            });
        }

        [Test]
        public void GivenPiplelineWithThreeStages_WhenHandlingRequest_ThenAllThreeHandlersAreCalledInOrder()
        {
            var testHandlerA = new TestHandler<TestRequestA>();
            var testHandlerB = new TestHandler<TestRequestA>();
            var testHandlerC = new TestHandler<TestRequestA>();

            var pipeline = PipelineDefinitionBuilder<TestRequestA, TestResponse>
                .StartWith(testHandlerA)
                .ThenWith(testHandlerB)
                .ThenWith(testHandlerC)
                .Build();

            var testRequest = new TestRequestA();

            pipeline.Handle(testRequest);

            var callOrder = new[]
                {
                    testHandlerA,
                    testHandlerB,
                    testHandlerC
                }.OrderBy(x => x._timestamp)
                .ToArray();

            Assert.Multiple(() =>
            {
                Assert.That(callOrder[0], Is.EqualTo(testHandlerA));
                Assert.That(callOrder[1], Is.EqualTo(testHandlerB));
                Assert.That(callOrder[2], Is.EqualTo(testHandlerC));
            });
        }

        [Test]
        public void GivenPipelineThatChangesRequest_WhenHandlingRequest_ThenChangesRequestInPipeline()
        {
            var testHandlerA = new TestHandler<TestRequestA>();
            var testHandlerB = new TestHandler<TestRequestA>();
            var testMutationHandler = new TestMutationHandler();
            var testHandlerC = new TestHandler<TestRequestB>();


            var pipeline = PipelineDefinitionBuilder<TestRequestA, TestResponse>
                .StartWith(testHandlerA)
                .ThenWith(testHandlerB)
                .ThenWithMutation(testMutationHandler)
                .ThenWith(testHandlerC)
                .Build();

            var testRequest = new TestRequestA();

            pipeline.Handle(testRequest);

            Assert.Multiple(() =>
            {
                Assert.That(testHandlerA._request, Is.EqualTo(testRequest));
                Assert.That(testHandlerB._request, Is.EqualTo(testRequest));
                Assert.That(testMutationHandler._request, Is.EqualTo(testRequest));
                Assert.That(testHandlerC._request, Is.Not.EqualTo(testRequest));
                Assert.That(testHandlerC._request, Is.TypeOf<TestRequestB>());
            });
        }
    }
}