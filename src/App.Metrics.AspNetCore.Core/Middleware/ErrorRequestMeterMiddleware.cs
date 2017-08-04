// <copyright file="ErrorRequestMeterMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Middleware
{
    /// <summary>
    ///     Measures the overall error request rate as well as the rate per endpoint.
    ///     Also measures these error rates per OAuth2 Client as a separate metric
    /// </summary>
    // ReSharper disable ClassNeverInstantiated.Global
    public class ErrorRequestMeterMiddleware : AppMetricsMiddleware<MetricsAspNetCoreOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        public ErrorRequestMeterMiddleware(
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
            try
            {
                Logger.MiddlewareExecuting(GetType());

                await Next(context);

                if (PerformMetric(context))
                {
                    var routeTemplate = context.GetMetricsCurrentRouteName();

                    if (!context.Response.IsSuccessfulResponse() && ShouldTrackHttpStatusCode(context.Response.StatusCode))
                    {
                        Metrics.RecordHttpRequestError(routeTemplate, context.Response.StatusCode);
                    }
                }
            }
            catch (Exception exception)
            {
                if (!PerformMetric(context))
                {
                    throw;
                }

                var routeTemplate = context.GetMetricsCurrentRouteName();

                Metrics.RecordHttpRequestError(routeTemplate, (int)HttpStatusCode.InternalServerError);
                Metrics.RecordException(routeTemplate, exception.GetType().FullName);

                throw;
            }
            finally
            {
                Logger.MiddlewareExecuted(GetType());
            }
        }
    }
}