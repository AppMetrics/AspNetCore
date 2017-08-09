// <copyright file="MetricsAspNetCoreMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Internal;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics AspNet Core services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsAspNetCoreMetricsBuilderExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics AspNet Core services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IMetricsAspNetCoreBuilder"/> that can be used to further configure the App Metrics AspNet Core services.</returns>
        public static IMetricsAspNetCoreBuilder AddAspNetCoreMetrics(this IMetricsBuilder builder)
        {
            builder.Services.AddAspNetCoreMetricsCore();

            return new MetricsAspNetCoreBuilder(builder.Services);
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreOptions" />.</param>
        /// <returns>An <see cref="IMetricsAspNetCoreBuilder"/> that can be used to further configure the App Metrics AspNet Core services.</returns>
        public static IMetricsAspNetCoreBuilder AddAspNetCoreMetrics(
            this IMetricsBuilder builder,
            IConfiguration configuration)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreMetrics();

            builder.Services.Configure<MetricsAspNetCoreOptions>(configuration);

            return aspNetCoreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsAspNetCoreOptions" />.
        /// </param>
        /// <returns>An <see cref="IMetricsAspNetCoreBuilder"/> that can be used to further configure the App Metrics AspNet Core services.</returns>
        public static IMetricsAspNetCoreBuilder AddAspNetCoreMetrics(
            this IMetricsBuilder builder,
            IConfiguration configuration,
            Action<MetricsAspNetCoreOptions> setupAction)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreMetrics();

            builder.Services.Configure<MetricsAspNetCoreOptions>(configuration);
            builder.Services.Configure(setupAction);

            return aspNetCoreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsAspNetCoreOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreOptions" />.</param>
        /// <returns>An <see cref="IMetricsAspNetCoreBuilder"/> that can be used to further configure the App Metrics AspNet Core services.</returns>
        public static IMetricsAspNetCoreBuilder AddAspNetCoreMetrics(
            this IMetricsBuilder builder,
            Action<MetricsAspNetCoreOptions> setupAction,
            IConfiguration configuration)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreMetrics();

            builder.Services.Configure(setupAction);
            builder.Services.Configure<MetricsAspNetCoreOptions>(configuration);

            return aspNetCoreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsAspNetCoreOptions" />.
        /// </param>
        /// <returns>An <see cref="IMetricsAspNetCoreBuilder"/> that can be used to further configure the App Metrics AspNet Core services.</returns>
        public static IMetricsAspNetCoreBuilder AddAspNetCoreMetrics(
            this IMetricsBuilder builder,
            Action<MetricsAspNetCoreOptions> setupAction)
        {
            var aspNetCoreBuilder = builder.AddAspNetCoreMetrics();

            builder.Services.Configure(setupAction);

            return aspNetCoreBuilder;
        }
    }
}