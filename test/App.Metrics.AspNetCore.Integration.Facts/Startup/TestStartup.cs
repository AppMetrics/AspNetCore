// <copyright file="TestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.TrackingMiddleware;
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
            MetricsTrackingMiddlewareOptions trackingOptions,
            MetricsEndpointsOptions endpointsOptions,
            IFilterMetrics filter = null)
        {
            services.AddLogging().AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            var builder = services.AddMetricsCore(
                                       options =>
                                       {
                                           options.DefaultContextLabel = appMetricsOptions.DefaultContextLabel;
                                           options.MetricsEnabled = appMetricsOptions.MetricsEnabled;
                                           options.DefaultOutputMetricsFormatter = appMetricsOptions.DefaultOutputMetricsFormatter;
                                           options.DefaultOutputEnvFormatter = appMetricsOptions.DefaultOutputEnvFormatter;
                                       }).AddDefaultReservoir(() => new DefaultAlgorithmRReservoir(1028)).AddClockType<TestClock>();

            builder.AddJsonFormatters();

            services.AddAspNetCoreMetricsCore().
                     AddTrackingMiddlewareOptionsCore(
                         options =>
                         {
                             options.OAuth2TrackingEnabled = trackingOptions.OAuth2TrackingEnabled;
                             options.IgnoredRoutesRegexPatterns = trackingOptions.IgnoredRoutesRegexPatterns;
                             options.IgnoredHttpStatusCodes = trackingOptions.IgnoredHttpStatusCodes;
                         }).
                     AddEndpointOptionsCore(
                         options =>
                         {
                             options.MetricsTextEndpointEnabled = endpointsOptions.MetricsTextEndpointEnabled;
                             options.MetricsEndpointEnabled = endpointsOptions.MetricsEndpointEnabled;
                             options.PingEndpointEnabled = endpointsOptions.PingEndpointEnabled;
                             options.MetricsEndpoint = endpointsOptions.MetricsEndpoint;
                             options.MetricsTextEndpoint = endpointsOptions.MetricsTextEndpoint;
                             options.PingEndpoint = endpointsOptions.PingEndpoint;
                         });

            if (filter != null)
            {
                builder.AddGlobalFilter(filter);
            }
        }
    }
}