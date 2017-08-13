// <copyright file="PingTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public class PingTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UsePingEndpoint();

            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new MetricsOptions
                                    {
                                        DefaultContextLabel = "testing",
                                        MetricsEnabled = true
                                    };

            var endpointsOptions = new MetricsEndpointsOptions
                                   {
                                       MetricsTextEndpointEnabled = true,
                                       MetricsEndpointEnabled = true,
                                       PingEndpointEnabled = true
                                   };

            var aspNetCoreOptions = new MetricsAspNetCoreOptions();

            SetupServices(services, appMetricsOptions, aspNetCoreOptions, endpointsOptions);
        }
    }
}