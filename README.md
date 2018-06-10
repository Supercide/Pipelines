# Pipelines [![Build status](https://ci.appveyor.com/api/projects/status/d7de0vpggjtmju9m?svg=true)](https://ci.appveyor.com/project/jordan-Anderson/pipelines) [![NuGet](https://img.shields.io/nuget/v/Anderson.Pipelines.svg)](https://www.nuget.org/packages/Anderson.Pipelines)

Pipelines allows you to creates custom processing piplelines with no dependencies.
It supports pipelines that can mutate the original request into a different type for example

`HelloWorld -> Pipe(HelloWorld) -> MutationPipe(HelloWorld) -> Pipe(GoodBye)`

I created this project because I needed to secure one of my azue functions with 
JWT tokens. Securing the function was the easy part, the part i found messy 
was seperating out the different authorization, validation and business logic 
concerns inside the function. Piplines allows us to create pipes that are only 
conerned about doing one thing. Logic that would otherwise be complex becomes 
reuseable, testable and simple as they are broken out into seperate concerns.

## Installing Pipelines

Install Pipelines with nuget 

```
Install-Package Pipelines
```

# Setting up
To Create your own custom pipeline you will need to create a class that 
inherits from `PipelineDefinition<TRequest, TestResponse>`. For this 
example we will create a basic validation pipe. Lets start with creating 
our request and response objects.

```C#
    public class Order
    {
        public string Name { get; set; }
        public string ItemNumber { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
    }

    public class OrderDispatched
    {
        public DateTime Dispatched { get; set; }
    }

    public enum OrderError
    {
        Unknown,
        Validation
    }
```

Next we will create a validation Pipe and the OrderDispatch Pipe. The `Response<>` objects will allow us
to return either a successful response or an error. Each pipe has a `InnerHandler`
and a `Handle` method. The `InnerHandler` method advances to the next pipe 
in the chain when called.

```C#
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
```

To construct the pipeline we use the PipelineDefinitionBuilder calling build
when we have plugged all the pipes in place. Pipes will get called in the same
order they was registered in.

```C#
var pipeline = PipelineDefinitionBuilder<Order, Response<OrderDispatched, OrderError>>
                .StartWith(new Validation())
                .ThenWith(new OrderDispatchedHandler())
                .Build();
```
 
The last thing to do is to call our pipeline. If we call our pipeline with a 
missing address then our pipeline will return the OrderError.Validation. If we
call our pipeline with an address then the OrderDispatched will be returned.
```C#
var response = pipeline.Handle(new Order
            {
                Address = "32 north bridge",
                Amount = 20m,
                ItemNumber = "12234BDC",
                Name = "jordan"
            });
// Returns OrderDispatched

var response = pipeline.Handle(new Order());

// Returns OrderError.Validation
```

The best way to understand the project and concepts is to look and run the unit
tests [Click Here](./tests/Pipelines.Tests)
