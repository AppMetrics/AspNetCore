﻿// <copyright file="PostAndPutRequestSizeHistogramMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Middleware.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global

    public class PostAndPutRequestSizeHistogramMiddleware : AppMetricsMiddleware<AppMetricsMiddlewareOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public PostAndPutRequestSizeHistogramMiddleware(
            RequestDelegate next,
            AppMetricsMiddlewareOptions appMiddlewareOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, appMiddlewareOptions, loggerFactory, metrics)
        {
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                var httpMethod = context.Request.Method.ToUpperInvariant();

                if (httpMethod == "POST")
                {
                    if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                    {
                        Metrics.UpdatePostRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                }
                else if (httpMethod == "PUT")
                {
                    if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                    {
                        Metrics.UpdatePutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()));
                    }
                }

                Logger.MiddlewareExecuted(GetType());
            }

            await Next(context);
        }
    }
}