﻿// <copyright file="MetricsAspNetCoreServicesHelper.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.AspNetCore.DependencyInjection.Internal
{
    public static class MetricsAspNetCoreServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(MetricsAspNetCoreMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddAspNetCoreMetricsCore()\nIServiceCollection.AddAspNetCoreMetrics()\n");
            }
        }
    }
}