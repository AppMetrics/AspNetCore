// <copyright file="PingApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
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

            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsOptions>>();

            UsePingMiddleware(app, endpointsOptionsAccessor);

            return app;
        }

        private static bool ShouldUsePing(IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor, HttpContext context)
        {
            return context.Request.Path == endpointsOptionsAccessor.Value.PingEndpoint &&
                   endpointsOptionsAccessor.Value.PingEndpointEnabled &&
                   endpointsOptionsAccessor.Value.PingEndpoint.IsPresent();
        }

        private static void UsePingMiddleware(IApplicationBuilder app, IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor)
        {
            app.UseWhen(
                context => ShouldUsePing(endpointsOptionsAccessor, context),
                appBuilder => { appBuilder.UseMiddleware<PingEndpointMiddleware>(); });
        }
    }
}