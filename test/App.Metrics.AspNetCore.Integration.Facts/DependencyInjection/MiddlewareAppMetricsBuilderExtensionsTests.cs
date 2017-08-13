// <copyright file="MiddlewareAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Endpoints;
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
            var options = new MetricsAspNetCoreOptions();
            var endpointOptions = new MetricsEndpointsOptions();

            var provider = SetupServicesAndConfiguration();
            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>().Value; };
            Action resolveEndpointsOptions = () => { endpointOptions = provider.GetRequiredService<IOptions<MetricsEndpointsOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            resolveEndpointsOptions.ShouldNotThrow();

            options.ApdexTrackingEnabled.Should().Be(false);
            options.ApdexTSeconds.Should().Be(0.8);
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
            var options = new MetricsAspNetCoreOptions();
            var provider = SetupServicesAndConfiguration(
                (o) =>
                {
                    o.ApdexTSeconds = 0.7;
                    o.ApdexTrackingEnabled = true;
                });

            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            options.ApdexTrackingEnabled.Should().Be(true);
            options.ApdexTSeconds.Should().Be(0.7);
        }

        private IServiceProvider SetupServicesAndConfiguration(
            Action<MetricsAspNetCoreOptions> setupAction = null,
            Action<MetricsEndpointsOptions> setupEndpointAction = null)
        {
            var services = new ServiceCollection();
            services.AddOptions();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var metricsBuilder = services.AddMetrics();
            IMetricsAspNetCoreBuilder aspNetCoreBuilder;

            if (setupAction == null)
            {
                aspNetCoreBuilder = metricsBuilder.AddAspNetCoreMetrics(configuration.GetSection("MetricsAspNetCoreOptions"));
            }
            else
            {
                aspNetCoreBuilder = metricsBuilder.AddAspNetCoreMetrics(
                    configuration.GetSection("MetricsAspNetCoreOptions"),
                    setupAction);
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