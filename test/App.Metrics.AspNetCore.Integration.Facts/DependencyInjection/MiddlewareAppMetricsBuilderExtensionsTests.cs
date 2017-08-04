// <copyright file="MiddlewareAppMetricsBuilderExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
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

            var provider = SetupServicesAndConfiguration();
            Action resolveOptions = () => { options = provider.GetRequiredService<IOptions<MetricsAspNetCoreOptions>>().Value; };

            resolveOptions.ShouldNotThrow();
            options.ApdexTrackingEnabled.Should().Be(false);
            options.ApdexTSeconds.Should().Be(0.8);
            options.MetricsEndpoint.Should().Be("/metrics-test");
            options.MetricsTextEndpoint.Should().Be("/metrics-text-test");
            options.PingEndpoint.Should().Be("/ping-test");
            options.MetricsTextEndpointEnabled.Should().Be(false);
            options.MetricsEndpointEnabled.Should().Be(false);
            options.PingEndpointEnabled.Should().Be(false);
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
            Action<MetricsAspNetCoreOptions> setupAction = null)
        {
            var services = new ServiceCollection();
            services.AddOptions();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("DependencyInjection/TestConfiguration/appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var metricsBuilder = services.AddMetrics();

            if (setupAction == null)
            {
                metricsBuilder.AddAspNetCoreMetrics(configuration.GetSection("MetricsAspNetCoreOptions"));
            }
            else
            {
                metricsBuilder.AddAspNetCoreMetrics(
                    configuration.GetSection("MetricsAspNetCoreOptions"),
                    setupAction);
            }

            return services.BuildServiceProvider();
        }
    }
}