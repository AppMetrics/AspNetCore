// <copyright file="PingApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics ping pong to the request execution
    ///     pipeline.
    /// </summary>
    public static class PingApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Ping middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UsePingEndpoint(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            UsePingMiddleware(app, metricsAspNetCoreOptionsAccessor);

            return app;
        }

        private static bool ShouldUsePing(IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor, HttpContext context)
        {
            return context.Request.Path == metricsAspNetCoreOptionsAccessor.Value.PingEndpoint &&
                   metricsAspNetCoreOptionsAccessor.Value.PingEndpointEnabled &&
                   metricsAspNetCoreOptionsAccessor.Value.PingEndpoint.IsPresent();
        }

        private static void UsePingMiddleware(IApplicationBuilder app, IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor)
        {
            app.UseWhen(
                context => ShouldUsePing(metricsAspNetCoreOptionsAccessor, context),
                appBuilder => { appBuilder.UseMiddleware<PingEndpointMiddleware>(); });
        }
    }
}