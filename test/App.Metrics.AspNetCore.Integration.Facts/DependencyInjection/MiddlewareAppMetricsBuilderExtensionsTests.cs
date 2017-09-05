// <copyright file="MiddlewareAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.TrackingMiddleware;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.DependencyInjection
{
    public class MiddlewareAppMetricsBuilderExtensionsTests
    {
        [Fact]
        public void Can_load_settings_from_configuration()
        {
            var trackingOptions = new MetricsTrackingMiddlewareOptions();
            var endpointOptions = new MetricsEndpointsOptions();

            var provider = SetupServicesAndConfiguration();
            Action resolveOptions = () => { trackingOptions = provider.GetRequiredService<IOptions<MetricsTrackingMiddlewareOptions>>().Value; };
            Action resolveEndpointsOptions = () => { endpointOptions = provider.GetRequiredService<IOptions<MetricsEndpointsOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            resolveEndpointsOptions.ShouldNotThrow();

            trackingOptions.ApdexTrackingEnabled.Should().Be(false);
            trackingOptions.ApdexTSeconds.Should().Be(0.8);
            endpointOptions.MetricsEndpoint.Should().Be("/metrics-test");
            endpointOptions.MetricsTextEndpoint.Should().Be("/metrics-text-test");
            endpointOptions.PingEndpoint.Should().Be("/ping-test");
            endpointOptions.MetricsTextEndpointEnabled.Should().Be(false);
            endpointOptions.MetricsEndpointEnabled.Should().Be(false);
            endpointOptions.PingEndpointEnabled.Should().Be(false);
        }

        [Fact]
        public void Can_override_settings_from_configuration()
        {
            var options = new MetricsTrackingMiddlewareOptions();
            var provider = SetupServicesAndConfiguration(
                (o) =>
                {
                    o.ApdexTSeconds = 0.7;
                    o.ApdexTrackingEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<MetricsTrackingMiddlewareOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            options.ApdexTrackingEnabled.Should().Be(true);
            options.ApdexTSeconds.Should().Be(0.7);
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<MetricsTrackingMiddlewareOptions> trackingSetupAction = null,
            Action<MetricsEndpointsOptions> setupEndpointAction = null)
        {
            var services = new ServiceCollection();
            services.AddOptions();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            services.AddMetrics();

            var aspNetCoreBuilder = services.AddAspNetCoreMetrics();

            if (trackingSetupAction == null)
            {
                aspNetCoreBuilder.AddTrackingMiddlewareOptions(configuration.GetSection("MetricsTrackingMiddlewareOptions"));
            }
            else
            {
                aspNetCoreBuilder.AddTrackingMiddlewareOptions(
                    configuration.GetSection("MetricsTrackingMiddlewareOptions"),
                    trackingSetupAction);
            }

            if (setupEndpointAction == null)
            {
                aspNetCoreBuilder.AddEndpointOptions(configuration.GetSection("MetricsEndpointsOptions"));
            }
            else
            {
                aspNetCoreBuilder.AddEndpointOptions(configuration.GetSection("MetricsEndpointsOptions"), setupEndpointAction);
            }

            return services.BuildServiceProvider();
        }
    }
}