// <copyright file="MetricsWebHostOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.TrackingMiddleware;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore
{
    /// <summary>
    ///     Provides programmatic configuration for metrics, metrics endpoints and tracking middleware in the App Metrics
    ///     framework.
    /// </summary>
    public class MetricsWebHostOptions
    {
        public MetricsWebHostOptions()
        {
            EndpointOptions = options => { };
            MetricsOptions = options => { };
            TrackingMiddlewareOptions = options => { };
        }

        /// <summary>
        ///     Gets the <see cref="IMetricsCoreBuilder" /> allowing configuration of additional core metrics services and options.
        /// </summary>
        public IMetricsCoreBuilder CoreBuilder { get; internal set; }

        /// <summary>
        ///     Gets or sets <see cref="Action{T}" /> to configure the provided <see cref="MetricsEndpointsOptions" />.
        /// </summary>
        public Action<MetricsEndpointsOptions> EndpointOptions { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="Action{MetricsOptions}" /> to configure the provided <see cref="MetricsOptions" />.
        /// </summary>
        public Action<MetricsOptions> MetricsOptions { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="Action{MetricsTrackingMiddlewareOptions}" /> to configure the provided
        ///     <see cref="MetricsTrackingMiddlewareOptions" />.
        /// </summary>
        public Action<MetricsTrackingMiddlewareOptions> TrackingMiddlewareOptions { get; set; }
    }
}