// <copyright file="MetricsEndpiontsMetricsAspNetCoreCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsEndpiontsMetricsAspNetCoreCoreBuilderExtensions
    {
        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }

        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsEndpointsOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure<MetricsEndpointsOptions>(configuration);
            builder.Services.Configure(setupAction);

            return builder;
        }

        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsEndpointsOptions> setupAction,
            IConfiguration configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);
            builder.Services.Configure<MetricsEndpointsOptions>(configuration);

            return builder;
        }

        public static IMetricsAspNetCoreCoreBuilder AddEndpointOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            IConfiguration configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<MetricsEndpointsOptions>(configuration);

            return builder;
        }
    }
}