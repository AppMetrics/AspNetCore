// <copyright file="IMetricsTextResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore
{
    public interface IMetricsTextResponseWriter
    {
        Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken));
    }
}