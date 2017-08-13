// <copyright file="DisabledMetricsEndpointStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public class DisabledMetricsEndpointStartup : TestStartup
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
                                        MetricsEnabled = true
                                    };

            var endpointsOptions = new MetricsEndpointsOptions
                                   {
                                       MetricsEndpointEnabled = false
                                   };

            var aspNetCoreOptions = new MetricsAspNetCoreOptions();

            SetupServices(services, appMetricsOptions, aspNetCoreOptions, endpointsOptions);
        }
    }
}