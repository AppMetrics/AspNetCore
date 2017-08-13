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
        public static IMetricsAspNetCoreBuilder AddTrackingMiddlewareOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddTrackingMiddlewareOptionsCore(setupAction);

            return builder;
        }

        public static IMetricsAspNetCoreBuilder AddTrackingMiddlewareOptions(
            this IMetricsAspNetCoreBuilder builder,
            IConfiguration configuration,
            Action<MetricsTrackingMiddlewareOptions> setupAction)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddTrackingMiddlewareOptionsCore(configuration, setupAction);

            return builder;
        }

        public static IMetricsAspNetCoreBuilder AddTrackingMiddlewareOptions(
            this IMetricsAspNetCoreBuilder builder,
            Action<MetricsTrackingMiddlewareOptions> setupAction,
            IConfiguration configuration)
        {
            var coreBuilder = new MetricsAspNetCoreCoreBuilder(builder.Services);

            coreBuilder.AddTrackingMiddlewareOptionsCore(setupAction, configuration);

            return builder;
        }

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