// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MetricsSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          // .UseMetrics(ConfigureMetricsOptions())
                          .UseMetrics()
                          .UseStartup<Startup>().Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static Action<MetricsWebHostOptions> ConfigureMetricsOptions()
        {
            return options =>
            {
                options.EndpointOptions = endpointsOptions =>
                {
                    endpointsOptions.MetricsEndpointOutputFormatter = new MetricsTextOutputFormatter();
                    endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsJsonOutputFormatter();
                    endpointsOptions.MetricsEndpoint = "/metrics2";
                };

                options.MetricsOptions = metricsOptions => { metricsOptions.MetricsEnabled = true; };

                options.TrackingMiddlewareOptions = middlewareOptions => { middlewareOptions.IgnoredHttpStatusCodes = new[] { 500 }; };
            };
        }
    }
}