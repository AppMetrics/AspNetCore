// <copyright file="MetricsAspNetCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Internal
{
    public class MetricsAspNetCoreBuilder : IMetricsAspNetCoreBuilder
    {
        public MetricsAspNetCoreBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}