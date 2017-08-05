// <copyright file="MetricsAspNetReportingWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.Reporting;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class MetricsAspNetReportingWebHostBuilderExtensions
    {
        /// <summary>
        ///     Runs the configured App Metrics Reporting options once the application has started.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="reportSetupAction">Allows configuration of reporters via the <see cref="IReporter"/></param>
        /// <returns>
        ///     A reference to this instance after the operation has completed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />
        /// </exception>
        public static IWebHostBuilder UseMetricsReporting(
            this IWebHostBuilder hostBuilder,
            Action<IReportFactory> reportSetupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddMetricsReporting(reportSetupAction);

                services.AddSingleton<IStartupFilter>(new MetricsReportingStartupFilter());
            });

            return hostBuilder;
        }
    }
}