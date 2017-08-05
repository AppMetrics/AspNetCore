// <copyright file="PostAndPutRequestSizeHistogramMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.TrackingMiddleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class PostAndPutRequestSizeHistogramMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PostAndPutRequestSizeHistogramMiddleware> _logger;
        private readonly IMetrics _metrics;

        public PostAndPutRequestSizeHistogramMiddleware(
            RequestDelegate next,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            ILogger<PostAndPutRequestSizeHistogramMiddleware> logger,
            IMetrics metrics)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _metrics = metrics;
        }

        // ReSharper disable UnusedMember.Global
        public Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting(GetType());

            var httpMethod = context.Request.Method.ToUpperInvariant();

            if (httpMethod == "POST")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    _metrics.UpdatePostRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                }
            }
            else if (httpMethod == "PUT")
            {
                if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                {
                    _metrics.UpdatePutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                }
            }

            _logger.MiddlewareExecuted(GetType());

            return _next(context);
        }
    }
}