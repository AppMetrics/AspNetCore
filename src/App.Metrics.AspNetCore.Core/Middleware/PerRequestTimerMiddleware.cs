// <copyright file="PerRequestTimerMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global

    public class PerRequestTimerMiddleware : AppMetricsMiddleware<MetricsAspNetCoreOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private const string TimerItemsKey = "__App.Metrics.PerRequestStartTime__";

        public PerRequestTimerMiddleware(
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
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                context.Items[TimerItemsKey] = Metrics.Clock.Nanoseconds;

                await Next(context);

                if (context.HasMetricsCurrentRouteName() && ShouldTrackHttpStatusCode(context.Response.StatusCode))
                {
                    var startTime = (long)context.Items[TimerItemsKey];
                    var elapsed = Metrics.Clock.Nanoseconds - startTime;

                    Metrics.RecordEndpointsRequestTime(
                        GetOAuthClientIdIfRequired(context),
                        context.GetMetricsCurrentRouteName(),
                        elapsed);
                }

                Logger.MiddlewareExecuted(GetType());
            }
            else
            {
                await Next(context);
            }
        }
    }
}