// <copyright file="EnvironmentInfoMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class EnvironmentInfoMiddleware : AppMetricsMiddleware<MetricsAspNetCoreOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly EnvironmentInfoProvider _environmentInfoProvider;
        private readonly IEnvResponseWriter _envResponseWriter;
        private readonly RequestDelegate _next;

        public EnvironmentInfoMiddleware(
            RequestDelegate next,
            IOptions<MetricsAspNetCoreOptions> metricsAspNetCoreOptionsAccessor,
            ILoggerFactory loggerFactory,
            IMetrics metrics,
            IEnvResponseWriter environmentInfoResponseWriter,
            EnvironmentInfoProvider environmentInfoProvider)
            : base(next, metricsAspNetCoreOptionsAccessor, loggerFactory, metrics)
        {
            _environmentInfoProvider = environmentInfoProvider;
            _envResponseWriter = environmentInfoResponseWriter ?? throw new ArgumentNullException(nameof(environmentInfoResponseWriter));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            if (Options.EnvironmentInfoEndpointEnabled && Options.EnvironmentInfoEndpoint.IsPresent() && Options.EnvironmentInfoEndpoint == context.Request.Path)
            {
                Logger.MiddlewareExecuting(GetType());

                await _envResponseWriter.WriteAsync(context, _environmentInfoProvider.Build(), context.RequestAborted).ConfigureAwait(false);

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await _next(context);
        }
    }
}
