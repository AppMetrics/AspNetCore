// <copyright file="MetricsEndpointTextEndpointMiddleware.cs" company="Allan Hardy">
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
    public class MetricsEndpointTextEndpointMiddleware : AppMetricsMiddleware<MetricsAspNetCoreOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly IMetricsTextResponseWriter _metricsTextResponseWriter;

        public MetricsEndpointTextEndpointMiddleware(
            RequestDelegate next,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IMetricsTextResponseWriter metricsTextResponseWriter)
            : base(next, metricsAspNetCoreOptionsAccessor, loggerFactory, metrics)
        {
            _metricsTextResponseWriter = metricsTextResponseWriter ?? throw new ArgumentNullException(nameof(metricsTextResponseWriter));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (Options.MetricsTextEndpointEnabled && Options.MetricsTextEndpoint.IsPresent() && Options.MetricsTextEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                await _metricsTextResponseWriter.WriteAsync(context, Metrics.Snapshot.Get(), context.RequestAborted).ConfigureAwait(false);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(context);
        }
    }
}