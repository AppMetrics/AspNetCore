// <copyright file="OAuthTrackingMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class OAuthTrackingMiddleware : AppMetricsMiddleware<MetricsAspNetCoreOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public OAuthTrackingMiddleware(
            RequestDelegate next,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, metricsAspNetCoreOptionsAccessor, loggerFactory, metrics)
        {
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            await Next(context);

            var clientid = GetOAuthClientIdIfRequired(context);

            if (PerformMetric(context) && clientid.IsPresent() && context.HasMetricsCurrentRouteName())
            {
                Logger.MiddlewareExecuting(GetType());

                var routeTemplate = context.GetMetricsCurrentRouteName();

                Metrics.RecordClientRequestRate(routeTemplate, clientid);

                if (!context.Response.IsSuccessfulResponse() && ShouldTrackHttpStatusCode(context.Response.StatusCode))
                {
                    Metrics.RecordClientHttpRequestError(routeTemplate, context.Response.StatusCode, clientid);
                }

                var httpMethod = context.Request.Method.ToUpperInvariant();

                if (httpMethod == "POST")
                {
                    if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                    {
                        Metrics.UpdateClientPostRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                    }
                }
                else if (httpMethod == "PUT")
                {
                    if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
                    {
                        Metrics.UpdateClientPutRequestSize(long.Parse(context.Request.Headers["Content-Length"].First()), clientid, routeTemplate);
                    }
                }

                Logger.MiddlewareExecuted(GetType());
            }
        }
    }
}