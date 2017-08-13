// <copyright file="MetricsAspNetCoreMetricsCoreServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.DependencyInjection.Internal;
using App.Metrics.AspNetCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsAspNetCoreMetricsCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core metrics
        ///     services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddAspNetCoreMetricsCore(this IServiceCollection services)
        {
            ConfigureDefaultServices(services);
            AddAspNetCoreMetricsServices(services);

            return new MetricsAspNetCoreCoreBuilder(services);
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreOptions" />.</param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core metrics
        ///     services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddAspNetCoreMetricsCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var coreBuilder = services.AddAspNetCoreMetricsCore();

            services.Configure<MetricsAspNetCoreOptions>(configuration);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsAspNetCoreOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core metrics
        ///     services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddAspNetCoreMetricsCore(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MetricsAspNetCoreOptions> setupAction)
        {
            var coreBuilder = services.AddAspNetCoreMetricsCore();

            services.Configure<MetricsAspNetCoreOptions>(configuration);
            services.Configure(setupAction);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsAspNetCoreOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsAspNetCoreOptions" />.</param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core metrics
        ///     services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddAspNetCoreMetricsCore(
            this IServiceCollection services,
            Action<MetricsAspNetCoreOptions> setupAction,
            IConfiguration configuration)
        {
            var coreBuilder = services.AddAspNetCoreMetricsCore();

            services.Configure(setupAction);
            services.Configure<MetricsAspNetCoreOptions>(configuration);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsAspNetCoreOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsAspNetCoreCoreBuilder" /> that can be used to further configure the App Metrics AspNet Core metrics
        ///     services.
        /// </returns>
        public static IMetricsAspNetCoreCoreBuilder AddAspNetCoreMetricsCore(
            this IServiceCollection services,
            Action<MetricsAspNetCoreOptions> setupAction)
        {
            var coreBuilder = services.AddAspNetCoreMetricsCore();

            services.Configure(setupAction);

            return coreBuilder;
        }

        internal static void AddAspNetCoreMetricsServices(IServiceCollection services)
        {
            //
            // Response Writers
            //
            services.TryAddSingleton<IEnvResponseWriter, DefaultEnvResponseWriter>();
            services.TryAddSingleton<IMetricsResponseWriter, DefaultMetricsResponseWriter>();

            //
            // Random Infrastructure
            //
            services.TryAddSingleton<MetricsAspNetCoreMarkerService, MetricsAspNetCoreMarkerService>();
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddRouting();
        }
    }
}