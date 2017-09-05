// <copyright file="EnvInfoApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.Extensions.DependencyInjection.Internal;
using App.Metrics.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
            EnsureMetricsAdded(app);

            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsOptions>>();

            UseEnvInfoMiddleware(app, endpointsOptionsAccessor);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics Environment Information middleware to the <see cref="IApplicationBuilder" /> request execution
        ///     pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="formatter">
        ///     Overrides all configured <see cref="IEnvOutputFormatter" />, matching on accept headers
        ///     won't apply.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseEnvInfoEndpoint(this IApplicationBuilder app, IEnvOutputFormatter formatter)
        {
            EnsureMetricsAdded(app);

            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsOptions>>();

            UseEnvInfoMiddleware(app, endpointsOptionsAccessor, formatter);

            return app;
        }

        private static void EnsureMetricsAdded(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling using middleware.
            // We use the MetricsMarkerService to make sure if all the services were added.
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);
        }

        private static IEnvResponseWriter GetEnvInfoResponseWriter(IServiceProvider serviceProvider, IEnvOutputFormatter formatter = null)
        {
            var formatters = serviceProvider.GetRequiredService<EnvFormatterCollection>();

            if (formatter != null)
            {
                var responseWriter = new DefaultEnvResponseWriter(formatter, formatters);
                return responseWriter;
            }

            var options = serviceProvider.GetRequiredService<IOptions<MetricsEndpointsOptions>>();
            return new DefaultEnvResponseWriter(options.Value.EnvInfoEndpointOutputFormatter, formatters);
        }

        private static bool ShouldUseEnvInfo(IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor, HttpContext context)
        {
            int? port = null;

            if (endpointsOptionsAccessor.Value.AllEndpointsPort.HasValue)
            {
                port = endpointsOptionsAccessor.Value.AllEndpointsPort.Value;
            }
            else if (endpointsOptionsAccessor.Value.EnvironmentInfoEndpointPort.HasValue)
            {
                port = endpointsOptionsAccessor.Value.EnvironmentInfoEndpointPort.Value;
            }

            return endpointsOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                   endpointsOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent() &&
                   context.Request.Path == endpointsOptionsAccessor.Value.EnvironmentInfoEndpoint &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static void UseEnvInfoMiddleware(IApplicationBuilder app, IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor, IEnvOutputFormatter formatter = null)
        {
            if (endpointsOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                endpointsOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => ShouldUseEnvInfo(endpointsOptionsAccessor, context),
                    appBuilder =>
                    {
                        var responseWriter = GetEnvInfoResponseWriter(appBuilder.ApplicationServices, formatter);
                        appBuilder.UseMiddleware<EnvInfoMiddleware>(responseWriter);
                    });
            }
        }
    }
}