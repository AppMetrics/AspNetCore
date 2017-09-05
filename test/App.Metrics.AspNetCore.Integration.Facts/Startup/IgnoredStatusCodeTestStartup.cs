// <copyright file="IgnoredStatusCodeTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.TrackingMiddleware;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public class IgnoredStatusCodeTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetricsEndpoint();
            app.UseMetricsAllMiddleware();

            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new MetricsOptions
                                    {
                                        DefaultContextLabel = "testing",
                                        Enabled = true
                                    };

            var endpointsOptions = new MetricsEndpointsOptions
                                   {
                                        MetricsTextEndpointEnabled = true,
                                       MetricsEndpointEnabled = true,
                                       PingEndpointEnabled = true
            };

            var trackingOptions = new MetricsTrackingMiddlewareOptions();
            trackingOptions.IgnoredHttpStatusCodes.Add(401);

            SetupServices(services, appMetricsOptions, trackingOptions, endpointsOptions);
        }
    }
}