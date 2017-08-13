// <copyright file="MetricsEndpiontsMetricsAspNetCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Internal;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsEndpiontsMetricsAspNetCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddEndpointOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddEndpointOptionsCore(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddEndpointOptions(
            this IMetricsAspNetCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsEndpointsOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddEndpointOptionsCore(configuration, setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddEndpointOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction,
            IConfiguration configuration)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddEndpointOptionsCore(setupAction, configuration);

            return builder;
        }

        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreBuilder AddEndpointOptions(
            this IMetricsAspNetCoreBuilder builder,
            IConfiguration configuration)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddEndpointOptionsCore(configuration);

            return builder;
        }
    }
}