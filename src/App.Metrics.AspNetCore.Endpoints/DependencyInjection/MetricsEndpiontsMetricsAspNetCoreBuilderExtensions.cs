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
        public static IMetricsAspNetCoreBuilder AddEndpointOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddEndpointOptionsCore(setupAction);

            return builder;
        }

        public static IMetricsAspNetCoreBuilder AddEndpointOptions(
            this IMetricsAspNetCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsEndpointsOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddEndpointOptionsCore(configuration, setupAction);

            return builder;
        }

        public static IMetricsAspNetCoreBuilder AddEndpointOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction,
            IConfiguration configuration)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddEndpointOptionsCore(setupAction, configuration);

            return builder;
        }

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