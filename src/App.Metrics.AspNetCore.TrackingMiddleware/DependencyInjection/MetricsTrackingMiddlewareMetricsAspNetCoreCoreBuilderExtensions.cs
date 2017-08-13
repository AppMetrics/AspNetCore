// <copyright file="MetricsTrackingMiddlewareMetricsAspNetCoreCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.TrackingMiddleware;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsTrackingMiddlewareMetricsAspNetCoreCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddTrackingMiddlewareOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddTrackingMiddlewareOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MetricsTrackingMiddlewareOptions>(configuration);
            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddTrackingMiddlewareOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction,
            IConfiguration configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure(setupAction);
            builder.Services.Configure<MetricsTrackingMiddlewareOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds metrics tracking middleware configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddTrackingMiddlewareOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            IConfiguration configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MetricsTrackingMiddlewareOptions>(configuration);

            return builder;
        }
    }
}