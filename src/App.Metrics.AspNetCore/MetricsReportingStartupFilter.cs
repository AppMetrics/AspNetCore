// <copyright file="MetricsReportingStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Reporting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore
{
    public class MetricsReportingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                // Verify if AddMetrics was done before calling UseMetricsEndpoints
                // We use the MetricsMarkerService to make sure if all the services were added.
                AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

                var options = app.ApplicationServices.GetRequiredService<IOptions<MetricsReportingOptions>>();

                if (!options.Value.ReportingEnabled)
                {
                    next(app);
                    return;
                }

                var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
                var reporter = app.ApplicationServices.GetRequiredService<IReporter>();
                var metrics = app.ApplicationServices.GetRequiredService<IMetrics>();

                lifetime.ApplicationStarted.Register(() => { Task.Run(() => reporter.RunReports(metrics, lifetime.ApplicationStopping), lifetime.ApplicationStopping); });

                next(app);
            };
        }
    }
}