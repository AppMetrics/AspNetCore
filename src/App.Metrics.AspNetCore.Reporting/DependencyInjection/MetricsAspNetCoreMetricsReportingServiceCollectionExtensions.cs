// <copyright file="MetricsAspNetCoreMetricsReportingServiceCollectionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.AspNetCore.Reporting;
using App.Metrics.Reporting;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Reporting services in an <see cref="IServiceCollection" />.
    /// </summary>
    [Obsolete("HostedService for metric reporting moved to App.Metrics.Extensions.Hosting now that HostedService is supported outside of a web context")]
    public static class MetricsAspNetCoreMetricsReportingServiceCollectionExtensions
    {
        public static void AddMetricsReportScheduler(
            this IServiceCollection services,
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler = null)
        {
            services.AddSingleton<IHostedService, ReportSchedulerHostedService>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<MetricsOptions>();
                var metrics = serviceProvider.GetRequiredService<IMetrics>();
                var reporters = serviceProvider.GetRequiredService<IReadOnlyCollection<IReportMetrics>>();

                var instance = new ReportSchedulerHostedService(metrics, options, reporters);

                if (unobservedTaskExceptionHandler != null)
                {
                    instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                }

                return instance;
            });
        }
    }
}
