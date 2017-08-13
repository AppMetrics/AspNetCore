// <copyright file="MetricsAspNetCoreMetricsOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Internal
{
    public class MetricsAspNetCoreMetricsOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsAspNetCoreOptions _aspNetCoreOptions;

        public MetricsAspNetCoreMetricsOptionsSetup(IOptions<MetricsAspNetCoreOptions> optionsAccessor)
        {
            _aspNetCoreOptions = optionsAccessor.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        /// <inheritdoc />
        public void Configure(MetricsOptions options)
        {
        }
    }
}