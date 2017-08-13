// <copyright file="MetricsTrackingMiddlewareMetricsAspNetCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.AspNetCore.TrackingMiddleware;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsTrackingMiddlewareMetricsAspNetCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsTrackingMiddlewareOptions}" /> to configure the provided <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddTrackingMiddlewareOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddTrackingMiddlewareOptionsCore(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreCoreBuilder" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsAspNetCoreCoreBuilder" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddTrackingMiddlewareOptions(
            this IMetricsAspNetCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddTrackingMiddlewareOptionsCore(configuration, setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsAspNetCoreCoreBuilder" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreCoreBuilder" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddTrackingMiddlewareOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction,
            IConfiguration configuration)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddTrackingMiddlewareOptionsCore(setupAction, configuration);

            return builder;
        }

        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreCoreBuilder" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddTrackingMiddlewareOptions(
            this IMetricsAspNetCoreBuilder builder,
            IConfiguration configuration)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddTrackingMiddlewareOptionsCore(configuration);

            return builder;
        }
    }
}