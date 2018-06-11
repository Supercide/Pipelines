﻿namespace Anderson.Pipelines.Handlers
{
    public interface IRequestHandler<in TRequest, out TResponse>
    {
        TResponse Handle(TRequest request);
    }
}