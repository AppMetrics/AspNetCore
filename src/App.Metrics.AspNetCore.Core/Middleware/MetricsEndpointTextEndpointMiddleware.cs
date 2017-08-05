// <copyright file="MetricsEndpointTextEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsEndpointTextEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly ILogger<MetricsEndpointTextEndpointMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly IMetricsTextResponseWriter _metricsTextResponseWriter;

        public MetricsEndpointTextEndpointMiddleware(
            RequestDelegate next,
            ILogger<MetricsEndpointTextEndpointMiddleware> logger,
            IMetrics metrics,
            IMetricsTextResponseWriter metricsTextResponseWriter)
        {
            _logger = logger;
            _metrics = metrics;
            _metricsTextResponseWriter = metricsTextResponseWriter ?? throw new ArgumentNullException(nameof(metricsTextResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting(GetType());

            await _metricsTextResponseWriter.WriteAsync(context, _metrics.Snapshot.Get(), context.RequestAborted);

            _logger.MiddlewareExecuted(GetType());
        }
    }
}