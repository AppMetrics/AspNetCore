// <copyright file="MetricsEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsEndpointMiddleware : AppMetricsMiddleware<MetricsAspNetCoreOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly IMetricsResponseWriter _metricsResponseWriter;
        private readonly RequestDelegate _next;

        public MetricsEndpointMiddleware(
            RequestDelegate next,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IMetricsResponseWriter metricsResponseWriter)
            : base(next, metricsAspNetCoreOptionsAccessor, loggerFactory, metrics)
        {
            _metricsResponseWriter = metricsResponseWriter ?? throw new ArgumentNullException(nameof(metricsResponseWriter));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (Options.MetricsEndpointEnabled && Options.MetricsEndpoint.IsPresent() && Options.MetricsEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                await _metricsResponseWriter.WriteAsync(context, Metrics.Snapshot.Get(), context.RequestAborted);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}