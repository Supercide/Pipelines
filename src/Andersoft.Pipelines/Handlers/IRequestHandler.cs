﻿using System.Threading;
using System.Threading.Tasks;
using Andersoft.Pipelines.Definitions;

namespace Andersoft.Pipelines.Handlers
{
    public interface IRequestHandler<in TRequest>
    {
        Task HandleAsync(TRequest request, Context context, CancellationToken token = default(CancellationToken));
    }
}