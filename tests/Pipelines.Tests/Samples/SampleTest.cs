using System.Threading.Tasks;
using Anderson.Pipelines.Builders;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;
using Anderson.Pipelines.Tests.Pipelines;
using NUnit.Framework;

namespace Anderson.Pipelines.Tests.Samples
{
    public class SampleTest
    {
        [Test]
        public async Task GivenValidOrder_WhenHandlingOrder_ThenOrderDispatchedIsReturned()
        {
            var pipeline = PipelineDefinitionBuilder
                .StartWith(new Validation())
                .ThenWith(new OrderDispatchedHandler())
                .Build();

            var request = new Order
            {
                Address = "32 north bridge",
                Amount = 20m,
                ItemNumber = "12234BDC",
                Name = "jordan"
            };

            var context = new Context();
            await pipeline.HandleAsync(request, context);

            Assert.That(context.GetResponse<OrderDispatched>(), Is.TypeOf<OrderDispatched>());
        }

        [Test]
        public async Task GivenOrder_WithMissingAddress_WhenHandlingOrder_ThenReturnsOrderValidationError()
        {
            var pipeline = PipelineDefinitionBuilder
                .StartWith(new Validation())
                .ThenWith(new OrderDispatchedHandler())
                .Build();

            var context = new Context();

            await pipeline.HandleAsync(new Order(), context);
            var response = context.GetError<OrderError>();
            Assert.That(response, Is.EqualTo(OrderError.Validation));
        }
    }
}
