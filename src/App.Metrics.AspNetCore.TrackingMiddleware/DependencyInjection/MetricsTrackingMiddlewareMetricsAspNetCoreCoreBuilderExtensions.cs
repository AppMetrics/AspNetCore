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
        public static IMetricsAspNetCoreCoreBuilder AddTrackingMiddlewareOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
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

        public static IMetricsAspNetCoreCoreBuilder AddTrackingMiddlewareOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure<MetricsTrackingMiddlewareOptions>(configuration);
            builder.Services.Configure(setupAction);

            return builder;
        }

        public static IMetricsAspNetCoreCoreBuilder AddTrackingMiddlewareOptionsCore(
            this IMetricsAspNetCoreCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction,
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
            builder.Services.Configure<MetricsTrackingMiddlewareOptions>(configuration);

            return builder;
        }

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