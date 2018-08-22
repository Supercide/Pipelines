using System.Linq;
using Andersoft.Pipelines.Builders;
using Andersoft.Pipelines.Definitions;
using NUnit.Framework;

namespace Andersoft.Pipelines.Tests.Pipelines
{
    public class PipelineTest
    {
        [Test]
        public void GivenPiplelineWithThreeStages_WhenHandlingRequest_ThenAllThreeHandlersHandleRequest()
        {
            var testHandlerA = new TestHandler<TestRequestA>();
            var testHandlerB = new TestHandler<TestRequestA>();
            var testHandlerC = new TestHandler<TestRequestA>();
            
            var pipeline = PipelineDefinitionBuilder
                .StartWith(testHandlerA)
                .ThenWith(testHandlerB)
                .ThenWith(testHandlerC)
                .Build();

            var testRequest = new TestRequestA();
            var context = new Context();
            pipeline.HandleAsync(testRequest, context);
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

            var pipeline = PipelineDefinitionBuilder
                .StartWith(testHandlerA)
                .ThenWith(testHandlerB)
                .ThenWith(testHandlerC)
                .Build();

            var testRequest = new TestRequestA();
            var context = new Context();
            pipeline.HandleAsync(testRequest, context);

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


            var pipeline = PipelineDefinitionBuilder
                .StartWith(testHandlerA)
                .ThenWith(testHandlerB)
                .ThenWithMutation(testMutationHandler)
                .ThenWith(testHandlerC)
                .Build();

            var testRequest = new TestRequestA();
            var context = new Context();
            pipeline.HandleAsync(testRequest, context);

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