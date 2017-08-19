// <copyright file="MetricsApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics to the request execution pipeline.
    /// </summary>
    public static class MetricsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics metrics endpoint middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <remarks>
        ///     Uses the mathcing <see cref="IMetricsOutputFormatter" /> given a requests Accept header, otherwise falls back
        ///     to MetricsOptions.DefaultOutputMetricsFormatter.
        /// </remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsEndpoint(this IApplicationBuilder app)
        {
            EnsureMetricsAdded(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsOptions>>();

            UseMetricsMiddleware(app, endpointsOptionsAccessor, metricsOptionsAccessor);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics metrics endpoint middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="formatter">
        ///     Overrides all configured <see cref="IMetricsOutputFormatter" />, matching on accept headers
        ///     won't apply.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsEndpoint(this IApplicationBuilder app, IMetricsOutputFormatter formatter)
        {
            EnsureMetricsAdded(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsOptions>>();

            UseMetricsMiddleware(app, endpointsOptionsAccessor, metricsOptionsAccessor, formatter);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics metrics text middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <remarks>By default uses the <see cref="MetricsTextOutputFormatter" /> to format results.</remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsTextEndpoint(this IApplicationBuilder app)
        {
            EnsureMetricsAdded(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsOptions>>();

            UseMetricsTextMiddleware(app, endpointsOptionsAccessor, metricsOptionsAccessor);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics metrics text middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="formatter">
        ///     Overrides the default use of <see cref="MetricsTextOutputFormatter" /> with the
        ///     <see cref="IMetricsOutputFormatter" /> specified.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsTextEndpoint(this IApplicationBuilder app, IMetricsOutputFormatter formatter)
        {
            EnsureMetricsAdded(app);

            var metricsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsOptions>>();

            UseMetricsTextMiddleware(app, endpointsOptionsAccessor, metricsOptionsAccessor, formatter);

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

        private static DefaultMetricsResponseWriter GetMetricsResponseWriter(IServiceProvider serviceProvider, IMetricsOutputFormatter formatter = null)
        {
            var options = serviceProvider.GetRequiredService<IOptions<MetricsOptions>>();
            var aspNetCoreOptions = serviceProvider.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();

            return formatter == null
                ? new DefaultMetricsResponseWriter(options, aspNetCoreOptions)
                : new DefaultMetricsResponseWriter(options, aspNetCoreOptions, formatter);
        }

        private static IMetricsResponseWriter GetMetricsTextResponseWriter(IServiceProvider serviceProvider, IMetricsOutputFormatter formatter = null)
        {
            var options = serviceProvider.GetRequiredService<IOptions<MetricsOptions>>();
            var aspNetCoreOptions = serviceProvider.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>();
            var textOptions = serviceProvider.GetRequiredService<IOptions<MetricsTextOptions>>();
            var textFormatter = formatter ?? new MetricsTextOutputFormatter(textOptions.Value);
            var responseWriter = new DefaultMetricsResponseWriter(options, aspNetCoreOptions, textFormatter);
            return responseWriter;
        }

        private static bool ShouldUseMetricsEndpoint(
            IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            HttpContext context)
        {
            int? port = null;

            if (endpointsOptionsAccessor.Value.AllEndpointsPort.HasValue)
            {
                port = endpointsOptionsAccessor.Value.AllEndpointsPort.Value;
            }
            else if (endpointsOptionsAccessor.Value.MetricsEndpointPort.HasValue)
            {
                port = endpointsOptionsAccessor.Value.MetricsEndpointPort.Value;
            }

            return context.Request.Path == endpointsOptionsAccessor.Value.MetricsEndpoint &&
                   endpointsOptionsAccessor.Value.MetricsEndpointEnabled &&
                   metricsOptionsAccessor.Value.MetricsEnabled &&
                   endpointsOptionsAccessor.Value.MetricsEndpoint.IsPresent() &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static bool ShouldUseMetricsTextEndpoint(
            IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            HttpContext context)
        {
            int? port = null;

            if (endpointsOptionsAccessor.Value.AllEndpointsPort.HasValue)
            {
                port = endpointsOptionsAccessor.Value.AllEndpointsPort.Value;
            }
            else if (endpointsOptionsAccessor.Value.MetricsTextEndpointPort.HasValue)
            {
                port = endpointsOptionsAccessor.Value.MetricsTextEndpointPort.Value;
            }

            return context.Request.Path == endpointsOptionsAccessor.Value.MetricsTextEndpoint &&
                   endpointsOptionsAccessor.Value.MetricsTextEndpointEnabled &&
                   metricsOptionsAccessor.Value.MetricsEnabled &&
                   endpointsOptionsAccessor.Value.MetricsTextEndpoint.IsPresent() &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static void UseMetricsMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IMetricsOutputFormatter formatter = null)
        {
            formatter = formatter ?? endpointsOptionsAccessor.Value.MetricsEndpointOutputFormatter;

            app.UseWhen(
                context => ShouldUseMetricsEndpoint(endpointsOptionsAccessor, metricsOptionsAccessor, context),
                appBuilder =>
                {
                    var responseWriter = GetMetricsResponseWriter(app.ApplicationServices, formatter);
                    appBuilder.UseMiddleware<MetricsEndpointMiddleware>(responseWriter);
                });
        }

        private static void UseMetricsTextMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsEndpointsOptions> endpointsOptionsAccessor,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IMetricsOutputFormatter formatter = null)
        {
            formatter = formatter ?? endpointsOptionsAccessor.Value.MetricsTextEndpointOutputFormatter;

            app.UseWhen(
                context => ShouldUseMetricsTextEndpoint(endpointsOptionsAccessor, metricsOptionsAccessor, context),
                appBuilder =>
                {
                    var responseWriter = GetMetricsTextResponseWriter(appBuilder.ApplicationServices, formatter);
                    appBuilder.UseMiddleware<MetricsEndpointMiddleware>(responseWriter);
                });
        }
    }
}