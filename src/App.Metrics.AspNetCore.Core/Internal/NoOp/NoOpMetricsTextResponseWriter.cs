// <copyright file="NoOpMetricsTextResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public class NoOpMetricsTextResponseWriter : IMetricsTextResponseWriter
    {
        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            return context.Response.WriteAsync("No formatter has been registered. See App.Metrics.Formatters.Ascii for example.", token);
        }
    }
}