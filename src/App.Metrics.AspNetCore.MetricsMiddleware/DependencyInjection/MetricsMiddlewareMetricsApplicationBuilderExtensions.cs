// <copyright file="MetricsMiddlewareMetricsApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.MetricsMiddleware;
using App.Metrics.DependencyInjection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics Middleware to the request execution
    ///     pipeline which measure typical web metrics.
    /// </summary>
    public static class MetricsMiddlewareMetricsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics active request tracking to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsActiveRequestMiddleware(this IApplicationBuilder app)
        {
            EnsureRequiredServices(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            UseMetricsgMiddleware<ActiveRequestCounterEndpointMiddleware>(app, metricsOptionsAccessor, metricsAspNetCoreOptionsAccessor);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics web tracking metrics to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsAllMiddleware(this IApplicationBuilder app)
        {
            EnsureRequiredServices(app);

            app.UseMetricsActiveRequestMiddleware();
            app.UseMetricsErrorTrackingMiddleware();
            app.UseMetricsPostAndPutSizeTrackingMiddleware();
            app.UseMetricsRequestTrackingMiddleware();
            app.UseMetricsOAuth2TrackingMiddleware();
            app.UseMetricsApdexTrackingMiddleware();

            return app;
        }

        /// <summary>
        ///     Adds App Metrics Apdex tracking to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsApdexTrackingMiddleware(this IApplicationBuilder app)
        {
            EnsureRequiredServices(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            app.UseWhen(
                context => !IsNotAnIgnoredRoute(metricsAspNetCoreOptionsAccessor.Value.IgnoredRoutesRegex, context.Request.Path) &&
                           context.HasMetricsCurrentRouteName() &&
                           metricsOptionsAccessor.Value.MetricsEnabled &&
                           metricsAspNetCoreOptionsAccessor.Value.ApdexTrackingEnabled,
                appBuilder => { appBuilder.UseMiddleware<ApdexMiddleware>(); });

            return app;
        }

        /// <summary>
        ///     Adds App Metrics error tracking to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsErrorTrackingMiddleware(this IApplicationBuilder app)
        {
            EnsureRequiredServices(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            UseMetricsgMiddleware<ErrorRequestMeterMiddleware>(app, metricsOptionsAccessor, metricsAspNetCoreOptionsAccessor);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics Apdex tracking to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsOAuth2TrackingMiddleware(this IApplicationBuilder app)
        {
            EnsureRequiredServices(app);

            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            app.UseWhen(
                context => !IsNotAnIgnoredRoute(metricsAspNetCoreOptionsAccessor.Value.IgnoredRoutesRegex, context.Request.Path) &&
                           context.OAuthClientId().IsPresent() &&
                           metricsAspNetCoreOptionsAccessor.Value.OAuth2TrackingEnabled,
                appBuilder => { appBuilder.UseMiddleware<OAuthTrackingMiddleware>(); });

            return app;
        }

        /// <summary>
        ///     Adds App Metrics POST and PUT request size tracking to the <see cref="IApplicationBuilder" /> request execution
        ///     pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsPostAndPutSizeTrackingMiddleware(this IApplicationBuilder app)
        {
            EnsureRequiredServices(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            UseMetricsgMiddleware<PostAndPutRequestSizeHistogramMiddleware>(app, metricsOptionsAccessor, metricsAspNetCoreOptionsAccessor);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics request tracking to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsRequestTrackingMiddleware(this IApplicationBuilder app)
        {
            EnsureRequiredServices(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            UseMetricsgMiddleware<RequestTimerMiddleware>(app, metricsOptionsAccessor, metricsAspNetCoreOptionsAccessor);
            UseMetricsgMiddleware<PerRequestTimerMiddleware>(app, metricsOptionsAccessor, metricsAspNetCoreOptionsAccessor);

            return app;
        }

        private static void EnsureRequiredServices(IApplicationBuilder app)
        {
            // Verify if AddMetrics was done before calling UseMetricsAllMiddleware
            // We use the MetricsMarkerService to make sure if all the services were added.
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);
        }

        private static bool IsNotAnIgnoredRoute(IReadOnlyList<Regex> ignoredRoutes, PathString currentPath)
        {
            if (ignoredRoutes.Any())
            {
                return ignoredRoutes.Any(ignorePattern => ignorePattern.IsMatch(currentPath.ToString().RemoveLeadingSlash()));
            }

            return false;
        }

        private static void UseMetricsgMiddleware<TMiddleware>(
            IApplicationBuilder app,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor)
        {
            app.UseWhen(
                context => !IsNotAnIgnoredRoute(metricsAspNetCoreOptionsAccessor.Value.IgnoredRoutesRegex, context.Request.Path) && metricsOptionsAccessor.Value.MetricsEnabled,
                appBuilder => { appBuilder.UseMiddleware<TMiddleware>(); });
        }
    }
}