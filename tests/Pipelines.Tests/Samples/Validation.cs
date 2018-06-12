using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;

namespace Anderson.Pipelines.Tests.Samples
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