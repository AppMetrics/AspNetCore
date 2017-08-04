// <copyright file="MetricsAspNetCoreMetricsAspNetCoreOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Internal
{
    public class MetricsAspNetCoreMetricsAspNetCoreOptionsSetup : IConfigureOptions<MetricsAspNetCoreOptions>
    {
        /// <inheritdoc />
        public void Configure(MetricsAspNetCoreOptions options)
        {
        }
    }
}