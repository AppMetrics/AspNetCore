// <copyright file="MetricsApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Middleware;
using App.Metrics.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics health to the request execution
    ///     pipeline.
    /// </summary>
    public static class MetricsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Health to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService to make sure if all the services were added.
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            //
            // Configure Endpoints
            //
            ConfigurePingMiddleware(app, metricsAspNetCoreOptionsAccessor);
            ConfigureMetricsTextMiddleware(app, metricsAspNetCoreOptionsAccessor, metricsOptionsAccessor);
            ConfigureMetricsMiddleware(app, metricsAspNetCoreOptionsAccessor, metricsOptionsAccessor);
            ConfigureEnvInfoMiddleware(app, metricsAspNetCoreOptionsAccessor);

            //
            // Configure Tracking
            //
            ConfigureMetricsTrackingMiddleware(app, metricsOptionsAccessor, metricsAspNetCoreOptionsAccessor);

            return app;
        }

        private static void ConfigureEnvInfoMiddleware(IApplicationBuilder app, IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor)
        {
            if (metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => context.Request.Path == metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpoint,
                    appBuilder => { appBuilder.UseMiddleware<EnvironmentInfoMiddleware>(); });
            }
        }

        private static void ConfigureMetricsMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            IOptions<MetricsOptions> metricsOptionsAccessor)
        {
            if (metricsAspNetCoreOptionsAccessor.Value.MetricsEndpointEnabled && metricsOptionsAccessor.Value.MetricsEnabled &&
                metricsAspNetCoreOptionsAccessor.Value.MetricsEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => context.Request.Path == metricsAspNetCoreOptionsAccessor.Value.MetricsEndpoint,
                    appBuilder => { appBuilder.UseMiddleware<MetricsEndpointMiddleware>(); });
            }
        }

        private static void ConfigureMetricsTextMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            IOptions<MetricsOptions> metricsOptionsAccessor)
        {
            if (metricsAspNetCoreOptionsAccessor.Value.MetricsTextEndpointEnabled && metricsOptionsAccessor.Value.MetricsEnabled &&
                metricsAspNetCoreOptionsAccessor.Value.MetricsTextEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => context.Request.Path == metricsAspNetCoreOptionsAccessor.Value.MetricsTextEndpoint,
                    appBuilder => { appBuilder.UseMiddleware<MetricsEndpointTextEndpointMiddleware>(); });
            }
        }

        private static void ConfigureMetricsTrackingMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor)
        {
            if (metricsOptionsAccessor.Value.MetricsEnabled && metricsAspNetCoreOptionsAccessor.Value.DefaultTrackingEnabled)
            {
                app.UseMiddleware<ActiveRequestCounterEndpointMiddleware>();
                app.UseMiddleware<ErrorRequestMeterMiddleware>();
                app.UseMiddleware<PerRequestTimerMiddleware>();
                app.UseMiddleware<OAuthTrackingMiddleware>();
                app.UseMiddleware<PostAndPutRequestSizeHistogramMiddleware>();
                app.UseMiddleware<RequestTimerMiddleware>();
            }

            if (metricsOptionsAccessor.Value.MetricsEnabled && metricsAspNetCoreOptionsAccessor.Value.ApdexTrackingEnabled)
            {
                app.UseMiddleware<ApdexMiddleware>();
            }
        }

        private static void ConfigurePingMiddleware(IApplicationBuilder app, IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor)
        {
            if (metricsAspNetCoreOptionsAccessor.Value.PingEndpointEnabled && metricsAspNetCoreOptionsAccessor.Value.PingEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => context.Request.Path == metricsAspNetCoreOptionsAccessor.Value.PingEndpoint,
                    appBuilder => { appBuilder.UseMiddleware<PingEndpointMiddleware>(); });
            }
        }
    }
}