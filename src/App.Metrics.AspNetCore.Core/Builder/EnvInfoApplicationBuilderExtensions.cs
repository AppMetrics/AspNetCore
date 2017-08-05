// <copyright file="EnvInfoApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics Environment Information to the request
    ///     execution pipeline.
    /// </summary>
    public static class EnvInfoApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Environment Information middleware to the <see cref="IApplicationBuilder" /> request execution
        ///     pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseEnvInfoEndpoint(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var metricsAspNetCoreOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            UseEnvInfoMiddleware(app, metricsAspNetCoreOptionsAccessor);

            return app;
        }

        private static void UseEnvInfoMiddleware(IApplicationBuilder app, IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor)
        {
            if (metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => context.Request.Path == metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpoint,
                    appBuilder => { appBuilder.UseMiddleware<EnvInfoMiddleware>(); });
            }
        }
    }
}