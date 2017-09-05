// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MetricsSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseMetrics()
                          .UseStartup<Startup>().Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        // private static Action<MetricsWebHostOptions> Configure()
        // {
        //     return options =>
        //     {
        //         options.EndpointOptions = endpointsOptions =>
        //         {
        //             endpointsOptions.MetricsEndpointOutputFormatter = new MetricsTextOutputFormatter();
        //             endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsJsonOutputFormatter();
        //             endpointsOptions.MetricsEndpoint = "/metrics2";
        //         };
        //         options.TrackingMiddlewareOptions = middlewareOptions => { middlewareOptions.IgnoredHttpStatusCodes = new[] { 500 }; };
        //     };
        // }
    }
}