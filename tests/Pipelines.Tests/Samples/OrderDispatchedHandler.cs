using System;
using Pipelines.Responses;

namespace Pipelines.Tests.Samples
{
    public class OrderDispatchedHandler : PipelineDefinition<Order, Response<OrderDispatched, OrderError>>
    {
        public override Response<OrderDispatched, OrderError> Handle(Order request)
        {
            return new OrderDispatched
            {
                Dispatched = DateTime.UtcNow
            };
        }
    }
}