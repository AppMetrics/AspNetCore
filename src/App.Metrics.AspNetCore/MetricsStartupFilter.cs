// <copyright file="MetricsStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Middleware;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.DependencyInjection.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore
{
    public class MetricsStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                // Verify if AddMetrics was done before calling UseMetrics
                // We use the MetricsMarkerService to make sure if all the services were added.
                AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

                var appMetricsOptions = app.ApplicationServices.GetRequiredService<AppMetricsOptions>();
                var appMetricsMiddlewareOptions = app.ApplicationServices.GetRequiredService<AppMetricsMiddlewareOptions>();

                if (appMetricsMiddlewareOptions.PingEndpointEnabled)
                {
                    app.UseMiddleware<PingEndpointMiddleware>();
                }

                if (appMetricsMiddlewareOptions.MetricsTextEndpointEnabled && appMetricsOptions.MetricsEnabled)
                {
                    app.UseMiddleware<MetricsEndpointTextEndpointMiddleware>();
                }

                if (appMetricsMiddlewareOptions.MetricsEndpointEnabled && appMetricsOptions.MetricsEnabled)
                {
                    app.UseMiddleware<MetricsEndpointMiddleware>();
                }

                if (appMetricsMiddlewareOptions.EnvironmentInfoEndpointEnabled)
                {
                    app.UseMiddleware<EnvironmentInfoMiddleware>();
                }

                if (appMetricsOptions.MetricsEnabled && appMetricsMiddlewareOptions.DefaultTrackingEnabled)
                {
                    app.UseMiddleware<ActiveRequestCounterEndpointMiddleware>();
                    app.UseMiddleware<ErrorRequestMeterMiddleware>();
                    app.UseMiddleware<PerRequestTimerMiddleware>();
                    app.UseMiddleware<OAuthTrackingMiddleware>();
                    app.UseMiddleware<PostAndPutRequestSizeHistogramMiddleware>();
                    app.UseMiddleware<RequestTimerMiddleware>();
                }

                if (appMetricsOptions.MetricsEnabled && appMetricsMiddlewareOptions.ApdexTrackingEnabled)
                {
                    app.UseMiddleware<ApdexMiddleware>();
                }

                next(app);
            };
        }
    }
}