// <copyright file="MetricsAspNetWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.TrackingMiddleware;
using App.Metrics.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics AspNet Core services in an <see cref="IWebHostBuilder" />.
    /// </summary>
    public static class MetricsAspNetWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetrics(this IWebHostBuilder hostBuilder)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var metricsOptions = new MetricsWebHostOptions { CoreBuilder = new MetricsCoreBuilder(services) };
                    var endpointsOptions = new MetricsEndpointsOptions();
                    context.Configuration.Bind(nameof(MetricsEndpointsOptions), endpointsOptions);

                    ConfigureServerUrlsKey(hostBuilder, endpointsOptions);
                    ConfigureMetricsServices(context, metricsOptions);
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsWebHostOptions}" /> to configure the provided <see cref="MetricsWebHostOptions" />.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetrics(this IWebHostBuilder hostBuilder, Action<MetricsWebHostOptions> setupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var metricsOptions = new MetricsWebHostOptions { CoreBuilder = new MetricsCoreBuilder(services) };
                    setupAction?.Invoke(metricsOptions);

                    var endpointsOptions = new MetricsEndpointsOptions();
                    context.Configuration.Bind(nameof(MetricsEndpointsOptions), endpointsOptions);
                    metricsOptions.EndpointOptions(endpointsOptions);

                    ConfigureServerUrlsKey(hostBuilder, endpointsOptions);
                    ConfigureMetricsServices(context, metricsOptions);
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{WebHostBuilderContext, MetricsWebHostOptions}" /> to configure the provided
        ///     <see cref="MetricsWebHostOptions" />.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetrics(this IWebHostBuilder hostBuilder, Action<WebHostBuilderContext, MetricsWebHostOptions> setupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var metricsOptions = new MetricsWebHostOptions { CoreBuilder = new MetricsCoreBuilder(services) };
                    setupAction?.Invoke(context, metricsOptions);

                    var endpointsOptions = new MetricsEndpointsOptions();
                    context.Configuration.Bind(nameof(MetricsEndpointsOptions), endpointsOptions);
                    metricsOptions.EndpointOptions(endpointsOptions);

                    ConfigureServerUrlsKey(hostBuilder, endpointsOptions);
                    ConfigureMetricsServices(context, metricsOptions);
                });

            return hostBuilder;
        }

        private static void ConfigureMetricsServices(
            WebHostBuilderContext context,
            MetricsWebHostOptions metricsOptions)
        {
            //
            // Add metrics services with options, setup action takes precedence over configuration section
            //
            var metricsBuilder = metricsOptions.CoreBuilder.Services.AddMetrics(context.Configuration.GetSection(nameof(MetricsOptions)), metricsOptions.MetricsOptions);

            //
            // Add metrics aspnet core essesntial services
            //
            var aspNetCoreMetricsBuilder = metricsBuilder.AddAspNetCoreMetrics(context.Configuration.GetSection(nameof(MetricsAspNetCoreOptions)));

            //
            // Add metrics endpoint options, setup action takes precedence over configuration section
            //
            aspNetCoreMetricsBuilder.AddEndpointOptions(context.Configuration.GetSection(nameof(MetricsEndpointsOptions)), metricsOptions.EndpointOptions);

            //
            // Add metrics tracking middleware options, setup action takes precedence over configuration section
            //
            aspNetCoreMetricsBuilder.AddTrackingMiddlewareOptions(
                context.Configuration.GetSection(nameof(MetricsTrackingMiddlewareOptions)),
                metricsOptions.TrackingMiddlewareOptions);

            //
            // Add the default metrics startup filter using all metrics tracking middleware and metrics endpoints
            //
            metricsOptions.CoreBuilder.Services.AddSingleton<IStartupFilter>(new DefaultMetricsStartupFilter());
        }

        private static void ConfigureServerUrlsKey(IWebHostBuilder hostBuilder, MetricsEndpointsOptions endpointsOptions)
        {
            var ports = new List<int>();

            if (endpointsOptions.AllEndpointsPort.HasValue)
            {
                ports.Add(endpointsOptions.AllEndpointsPort.Value);
            }
            else
            {
                if (endpointsOptions.MetricsEndpointPort.HasValue)
                {
                    ports.Add(endpointsOptions.MetricsEndpointPort.Value);
                }

                if (endpointsOptions.MetricsTextEndpointPort.HasValue)
                {
                    ports.Add(endpointsOptions.MetricsTextEndpointPort.Value);
                }

                if (endpointsOptions.PingEndpointPort.HasValue)
                {
                    ports.Add(endpointsOptions.PingEndpointPort.Value);
                }

                if (endpointsOptions.EnvironmentInfoEndpointPort.HasValue)
                {
                    ports.Add(endpointsOptions.EnvironmentInfoEndpointPort.Value);
                }
            }

            if (ports.Any())
            {
                var existingUrl = hostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
                var additionalUrls = string.Join(";", ports.Distinct().Select(p => $"http://localhost:{p}/"));
                hostBuilder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};{additionalUrls}");
            }
        }
    }
}