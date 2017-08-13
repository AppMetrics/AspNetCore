// <copyright file="EnvInfoApplicationBuilderExtensions.cs" company="Allan Hardy">
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

        private static bool ShouldUseEnvInfo(IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor, HttpContext context)
        {
            return metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                   metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent() &&
                   context.Request.Path == metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpoint;
        }

        private static void UseEnvInfoMiddleware(IApplicationBuilder app, IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor)
        {
            if (metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                metricsAspNetCoreOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => ShouldUseEnvInfo(metricsAspNetCoreOptionsAccessor, context),
                    appBuilder => { appBuilder.UseMiddleware<EnvInfoMiddleware>(); });
            }
        }
    }
}