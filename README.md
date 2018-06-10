# Pipelines [![Build status](https://ci.appveyor.com/api/projects/status/d7de0vpggjtmju9m?svg=true)](https://ci.appveyor.com/project/jordan-Anderson/pipelines) [![NuGet](https://img.shields.io/nuget/v/Anderson.Pipelines.svg)](https://www.nuget.org/packages/Anderson.Pipelines)

Pipelines allows you to creates custom processing piplelines with no dependencies.
It supports pipelines that can mutate the original request into a different type eg

`HelloWorld -> Pipe(HelloWorld) -> MutationPipe(HelloWorld) -> Pipe(GoodBye)`

See the wiki for further documentation

## Installing Pipelines

Install Pipelines with nuget 

```
Install-Package Pipelines
```