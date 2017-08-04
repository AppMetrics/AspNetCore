// <copyright file="ActiveRequestCounterEndpointMiddleware.cs" company="Allan Hardy">
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
    public class ActiveRequestCounterEndpointMiddleware : AppMetricsMiddleware<MetricsAspNetCoreOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public ActiveRequestCounterEndpointMiddleware(
            RequestDelegate next,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(next, metricsAspNetCoreOptionsAccessor, loggerFactory, metrics)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (PerformMetric(context))
            {
                Logger.MiddlewareExecuting(GetType());

                Metrics.IncrementActiveRequests();

                try
                {
                    await Next(context);
                    Metrics.DecrementActiveRequests();
                }
                catch (Exception)
                {
                    Metrics.DecrementActiveRequests();
                    throw;
                }
                finally
                {
                    Logger.MiddlewareExecuted(GetType());
                }
            }
            else
            {
                await Next(context);
            }
        }
    }
}