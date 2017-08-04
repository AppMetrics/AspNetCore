// <copyright file="TestStartup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            app.Use(
                (context, func) =>
                {
                    var clientId = string.Empty;

                    if (context.Request.Path.Value.Contains("oauth"))
                    {
                        clientId = context.Request.Path.Value.Split('/').Last();
                    }

                    if (!string.IsNullOrWhiteSpace(clientId))
                    {
                        context.User =
                            new ClaimsPrincipal(
                                new List<ClaimsIdentity>
                                {
                                    new ClaimsIdentity(
                                        new[]
                                        {
                                            new Claim("client_id", clientId)
                                        })
                                });
                    }

                    return func();
                });

            Metrics = app.ApplicationServices.GetRequiredService<IMetrics>();

            app.UseMetrics();

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

                        options.DefaultTrackingEnabled = metricsAspNetCoreOptions.DefaultTrackingEnabled;
                    });

            if (filter != null)
            {
                builder.AddGlobalFilter(filter);
            }
        }
    }
}