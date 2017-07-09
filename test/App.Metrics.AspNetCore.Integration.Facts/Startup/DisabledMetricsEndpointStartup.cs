﻿// <copyright file="DisabledMetricsEndpointStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Core.Configuration;
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
            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new AppMetricsOptions
                                    {
                                        MetricsEnabled = true
                                    };

            var appMetricsMiddlewareOptions = new AppMetricsMiddlewareOptions
                                       {
                                           MetricsEndpointEnabled = false
                                       };

            SetupServices(services, appMetricsOptions, appMetricsMiddlewareOptions);
        }
    }
}