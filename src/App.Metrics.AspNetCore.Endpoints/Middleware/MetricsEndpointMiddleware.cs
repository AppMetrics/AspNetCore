﻿// <copyright file="MetricsEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Endpoints.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly ILogger<MetricsEndpointMiddleware> _logger;
        private readonly IMetrics _metrics;
        private readonly IMetricsResponseWriter _metricsResponseWriter;

        public MetricsEndpointMiddleware(
            RequestDelegate next,
            ILogger<MetricsEndpointMiddleware> logger,
            IMetrics metrics,
            IMetricsResponseWriter metricsResponseWriter)
        {
            _logger = logger;
            _metrics = metrics;
            _metricsResponseWriter = metricsResponseWriter ?? throw new ArgumentNullException(nameof(metricsResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<MetricsEndpointMiddleware>();

            await _metricsResponseWriter.WriteAsync(context, _metrics.Snapshot.Get(), context.RequestAborted);

            _logger.MiddlewareExecuted<MetricsEndpointMiddleware>();
        }
    }
}