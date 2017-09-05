// <copyright file="MetricsEndpointsOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.Formatters;
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
        ///     Gets or sets the port to host available endpoints provided by App Metrics.
        /// </summary>
        /// <remarks>
        ///     This overrides all endpoing specific port configuration allowing a the port to be specific on a single
        ///     setting.
        /// </remarks>
        /// <value>
        ///     The App Metrics available endpoint's port.
        /// </value>
        public int? AllEndpointsPort { get; set; }

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
        ///     Gets or sets the port to host the env info endpoint.
        /// </summary>
        /// <value>
        ///     The env info endpoint's port.
        /// </value>
        public int? EnvironmentInfoEndpointPort { get; set; }

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
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics when the metrics endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsEndpointOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the port to host the metrics endpoint.
        /// </summary>
        /// <value>
        ///     The metrics endpoint's port.
        /// </value>
        public int? MetricsEndpointPort { get; set; }

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
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics when the metrics text endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsTextEndpointOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IEnvOutputFormatter" /> used to write environment information when the env endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IEnvOutputFormatter" /> used to write metrics.
        /// </value>
        public IEnvOutputFormatter EnvInfoEndpointOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the port to host the metrics text endpoint.
        /// </summary>
        /// <value>
        ///     The metrics text endpoint's port.
        /// </value>
        public int? MetricsTextEndpointPort { get; set; }

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

        /// <summary>
        ///     Gets or sets the port to host the ping endpoint.
        /// </summary>
        /// <value>
        ///     The pint endpoint's port.
        /// </value>
        public int? PingEndpointPort { get; set; }
    }
}