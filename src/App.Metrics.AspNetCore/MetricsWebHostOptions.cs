// <copyright file="MetricsWebHostOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;

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
            TrackingMiddlewareOptions = options => { };
        }

        /// <summary>
        ///     Gets or sets <see cref="Action{T}" /> to configure the provided <see cref="MetricEndpointsOptions" />.
        /// </summary>
        public Action<MetricEndpointsOptions> EndpointOptions { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="Action{MetricsWebTrackingOptions}" /> to configure the provided
        ///     <see cref="MetricsWebTrackingOptions" />.
        /// </summary>
        public Action<MetricsWebTrackingOptions> TrackingMiddlewareOptions { get; set; }
    }
}