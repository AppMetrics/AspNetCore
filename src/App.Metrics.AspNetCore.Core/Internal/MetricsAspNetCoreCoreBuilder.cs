// <copyright file="MetricsAspNetCoreCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Internal
{
    public class MetricsAspNetCoreCoreBuilder : IMetricsAspNetCoreCoreBuilder
    {
        public MetricsAspNetCoreCoreBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}
