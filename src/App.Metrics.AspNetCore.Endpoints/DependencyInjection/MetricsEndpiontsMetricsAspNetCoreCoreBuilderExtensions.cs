// <copyright file="MetricsEndpiontsMetricsAspNetCoreCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Endpoints.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsEndpiontsMetricsAspNetCoreCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction)
        {
            builder.Services.Configure(setupAction);
            AddRequiredEndpointOptions(builder.Services);

            return builder;
        }

        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsEndpointsOptions> setupAction)
        {
            builder.Services.Configure<MetricsEndpointsOptions>(configuration);
            builder.Services.Configure(setupAction);
            AddRequiredEndpointOptions(builder.Services);

            return builder;
        }

        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsEndpointsOptions}" /> to configure the provided <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction,
            IConfiguration configuration)
        {
            builder.Services.Configure(setupAction);
            builder.Services.Configure<MetricsEndpointsOptions>(configuration);
            AddRequiredEndpointOptions(builder.Services);

            return builder;
        }

        /// <summary>
        ///     Adds metrics endpoints configuration to the <see cref="IMetricsAspNetCoreCoreBuilder"/>
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsAspNetCoreCoreBuilder" /> to add configuration to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            IConfiguration configuration)
        {
            builder.Services.Configure<MetricsEndpointsOptions>(configuration);
            AddRequiredEndpointOptions(builder.Services);

            return builder;
        }

        private static void AddRequiredEndpointOptions(IServiceCollection services)
        {
            var descriptor = ServiceDescriptor.Transient<IConfigureOptions<MetricsEndpointsOptions>, MetricsEndpointsOptionsSetup>();
            services.TryAddEnumerable(descriptor);
        }
    }
}