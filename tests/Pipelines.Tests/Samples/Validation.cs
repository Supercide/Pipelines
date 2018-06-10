using Pipelines.Responses;

namespace Pipelines.Tests.Samples
{
    public class Validation : PipelineDefinition<Order, Response<OrderDispatched, OrderError>>
    {
        public override Response<OrderDispatched, OrderError> Handle(Order request)
        {
            if (string.IsNullOrEmpty(request.Address))
            {
                return OrderError.Validation;
            }

            return InnerHandler.Handle(request);
        }
    }
}