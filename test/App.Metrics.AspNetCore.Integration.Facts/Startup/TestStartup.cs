// <copyright file="TestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Filters;
using App.Metrics.ReservoirSampling.Uniform;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        protected IMetrics Metrics { get; private set; }

        protected void SetupAppBuilder(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Metrics = app.ApplicationServices.GetRequiredService<IMetrics>();

            app.UseMvc();
        }

        protected void SetupServices(
            IServiceCollection services,
            MetricsOptions appMetricsOptions,
            MetricsAspNetCoreOptions metricsAspNetCoreOptions,
            IFilterMetrics filter = null)
        {
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            var builder = services
                .AddMetricsCore(
                    options =>
                    {
                        options.DefaultContextLabel = appMetricsOptions.DefaultContextLabel;
                        options.MetricsEnabled = appMetricsOptions.MetricsEnabled;
                        options.DefaultOutputMetricsFormatter = appMetricsOptions.DefaultOutputMetricsFormatter;
                        options.DefaultOutputMetricsTextFormatter = appMetricsOptions.DefaultOutputMetricsTextFormatter;
                        options.DefaultOutputEnvFormatter = appMetricsOptions.DefaultOutputEnvFormatter;
                    })
                .AddDefaultReservoir(() => new DefaultAlgorithmRReservoir(1028))
                .AddClockType<TestClock>();

            builder.AddJsonFormatters();

            services.AddAspNetCoreMetricsCore(
                    options =>
                    {
                        options.MetricsTextEndpointEnabled = metricsAspNetCoreOptions.MetricsTextEndpointEnabled;
                        options.MetricsEndpointEnabled = metricsAspNetCoreOptions.MetricsEndpointEnabled;
                        options.PingEndpointEnabled = metricsAspNetCoreOptions.PingEndpointEnabled;
                        options.OAuth2TrackingEnabled = metricsAspNetCoreOptions.OAuth2TrackingEnabled;

                        options.MetricsEndpoint = metricsAspNetCoreOptions.MetricsEndpoint;
                        options.MetricsTextEndpoint = metricsAspNetCoreOptions.MetricsTextEndpoint;
                        options.PingEndpoint = metricsAspNetCoreOptions.PingEndpoint;

                        options.IgnoredRoutesRegexPatterns = metricsAspNetCoreOptions.IgnoredRoutesRegexPatterns;
                        options.IgnoredHttpStatusCodes = metricsAspNetCoreOptions.IgnoredHttpStatusCodes;
                    });

            if (filter != null)
            {
                builder.AddGlobalFilter(filter);
            }
        }
    }
}