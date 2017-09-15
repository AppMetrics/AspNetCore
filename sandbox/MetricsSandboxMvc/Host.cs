﻿// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            return WebHost.CreateDefaultBuilder(args)

                          #region App Metrics configuration options

                          // To configure ASP.NET core App Metrics hosting options: custom ports and endpoints
                          // .ConfigureAppMetricsHostingConfiguration(
                          //    options =>
                          //    {
                          //        options.AllEndpointsPort = 2222;
                          //    })

                          // To configure App Metrics core where an IMetricsBuilder is provided with defaults that can be overriden
                          // .ConfigureMetricsWithDefaults(
                          //     builder =>
                          //     {
                          //         builder.Configuration.Configure(
                          //             options =>
                          //             {
                          //                 options.DefaultContextLabel = "Testing";
                          //                 options.Enabled = true;
                          //             });
                          //     })

                          // To configure App Metrics core where an IMetricsBuilder is provided without any defaults set
                          // .ConfigureMetrics(
                          //     builder =>
                          //     {
                          //         builder.Configuration.Configure(
                          //             options =>
                          //             {
                          //                 options.DefaultContextLabel = "Testing";
                          //                 options.Enabled = true;
                          //             });
                          //     })

                          // To configure App Metrics web tracking and/or endpoints explicity
                          // .UseMetricsWebTracking()
                          // .UseMetricsEndpoints()

                          // To configure all App Metrics Asp.Net Core extensions overriding defaults
                          // .UseMetrics(Configure())

                          // To confgiure all App Metrics Asp.Net core extensions using a custom startup filter providing more control over what middleware tracking is added to the request pipeline for example
                          // .UseMetrics<DefaultMetricsStartupFilter>()

                          #endregion

                          .UseMetrics()
                          .UseSerilog()
                          .UseStartup<Startup>()
                          .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static Action<MetricsWebHostOptions> Configure()
        {
            return options =>
            {
                options.EndpointOptions = endpointsOptions =>
                {
                    endpointsOptions.MetricsEndpointOutputFormatter = new MetricsTextOutputFormatter();
                    endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsJsonOutputFormatter();
                };
                options.TrackingMiddlewareOptions = middlewareOptions => { middlewareOptions.IgnoredHttpStatusCodes = new[] { 500 }; };
            };
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .WriteTo.LiterateConsole(LogEventLevel.Verbose)
                .WriteTo.Seq("http://localhost:5341", LogEventLevel.Verbose)
                .CreateLogger();
        }
    }
}