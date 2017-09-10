// <copyright file="MetricsTrackingMiddlewareOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Tracking.Internal
{
    /// <summary>
    ///     Sets up default metric tracking middleware options for <see cref="MetricsWebTrackingOptions"/>.
    /// </summary>
    public class MetricsTrackingMiddlewareOptionsSetup : IConfigureOptions<MetricsWebTrackingOptions>
    {
        /// <inheritdoc />
        public void Configure(MetricsWebTrackingOptions options)
        {
        }
    }
}