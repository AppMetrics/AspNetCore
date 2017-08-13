// <copyright file="MetricsEndpointsOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Internal;
using Microsoft.AspNetCore.Builder;

namespace App.Metrics.AspNetCore.Endpoints
{
    /// <summary>
    ///     Provides programmatic configuration for metrics endpoints in the App Metrics framework.
    /// </summary>
    public class MetricsEndpointsOptions
    {
        public MetricsEndpointsOptions()
        {
            MetricsEndpointEnabled = true;
            MetricsTextEndpointEnabled = true;
            PingEndpointEnabled = true;
            EnvironmentInfoEndpointEnabled = true;
        }

        /// <summary>
        ///     Gets or sets the environment info endpoint, defaults to /env.
        /// </summary>
        /// <value>
        ///     The environment info endpoint.
        /// </value>
        public string EnvironmentInfoEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.EnvironmentInfoEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [environment info endpoint should be enabled], if disabled endpoint
        ///     responds with 404.
        /// </summary>
        /// <remarks>Only valid if UseEnvInfoEndpoint configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [environment info endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool EnvironmentInfoEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the metrics endpoint, defaults to /metrics.
        /// </summary>
        /// <value>
        ///     The metrics endpoint.
        /// </value>
        public string MetricsEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.MetricsEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics endpoint should be enabled], if disabled endpoint responds with
        ///     404.
        /// </summary>
        /// <remarks>Only valid if UseMetricsEndpoints configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [metrics endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the metrics text endpoint, defaults to metrics-text.
        /// </summary>
        /// <value>
        ///     The metrics text endpoint.
        /// </value>
        public string MetricsTextEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.MetricsTextEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics text endpoint should be enabled], if disabled endpoint responds
        ///     with 404.
        /// </summary>
        /// <remarks>Only valid if UseMetricsEndpoints configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [metrics text endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsTextEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the ping endpoint, defaults to /ping.
        /// </summary>
        /// <value>
        ///     The ping endpoint.
        /// </value>
        public string PingEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.PingEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [ping endpoint should be enabled], if disabled endpoint responds with 404.
        /// </summary>
        /// <remarks>Only valid if UsePingEndpoint configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [ping endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool PingEndpointEnabled { get; set; }
    }
}