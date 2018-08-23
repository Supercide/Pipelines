# Pipelines [![Build status](https://ci.appveyor.com/api/projects/status/d7de0vpggjtmju9m?svg=true)](https://ci.appveyor.com/project/jordan-Anderson/pipelines) [![NuGet](https://img.shields.io/nuget/v/Anderson.Pipelines.svg)](https://www.nuget.org/packages/Anderson.Pipelines)

Pipelines allows you to create custom processing pipelines with no dependencies.
With it, you can create a plug and play style pipelines that process information independently. For example

```
PurchaseCoffeData
        |
        V 
ProcessPayment(PurchaseCoffeData)
        |
        V
CreateOrderNote(PurchaseCoffeData)
        |
        V
GenerateReceipt(PurchaseCoffeData)
```

Each of these layers are totally independent of one another so extending the pipeline is as trivial as sticking another pipe in place.
The only requirement is that the input data is known at compile time. Even though input types have to be known at compile time, Pipelines is flexible enough for you to change the type of input data mid-flow. For example

```
GBP_USD.json
        |
        V 
MutateToOLHCData(GBP_USD.json)
        |
        V
CalculateVolatility(OLHCData)
        |
        V
StoreVolitility(OLHCData)
```

## Downloads
`Andersoft.Pipelines` can be found on NuGet along with any prerelease versions.
Install Pipelines with NuGet 

```
Install-Package Andersoft.Pipelines
```

## Documentation
Documentation is hosted on the GitHub Wiki for this project https://github.com/Supercide/Pipelines/wiki.

## Installing Pipelines

Install Pipelines with NuGet 

```
Install-Package Pipelines
```

## License
Andersoft.Pipelines is Open Source software released under the MIT license. You can use Andersoft.Pipelines in free and commercial applications and libraries without restrictions.
