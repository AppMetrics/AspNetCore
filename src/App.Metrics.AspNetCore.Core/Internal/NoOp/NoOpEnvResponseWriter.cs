// <copyright file="NoOpEnvResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public class NoOpEnvResponseWriter : IEnvResponseWriter
    {
        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default)
        {
            return context.Response.WriteAsync("No formatter has been registered. See App.Metrics.Formatters.Ascii for example.", token);
        }
    }
}