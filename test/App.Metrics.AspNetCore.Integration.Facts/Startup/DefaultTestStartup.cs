// <copyright file="DefaultTestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public class DefaultTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetricsEndpoints();
            app.UseMetricsAllMiddleware();

            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new MetricsOptions
                                    {
                                        DefaultContextLabel = "testing",
                                        MetricsEnabled = true,
                                        DefaultOutputMetricsFormatter = new MetricsJsonOutputFormatter(),
                                        DefaultOutputMetricsTextFormatter = new MetricsTextOutputFormatter()
                                    };

            var appMetricsMiddlewareOptions = new MetricsAspNetCoreOptions
                                       {
                                           MetricsTextEndpointEnabled = true,
                                           MetricsEndpointEnabled = true,
                                           PingEndpointEnabled = true
                                       };

            SetupServices(services, appMetricsOptions, appMetricsMiddlewareOptions);
        }
    }
}