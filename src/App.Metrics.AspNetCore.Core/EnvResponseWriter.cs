// <copyright file="EnvResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace App.Metrics.AspNetCore
{
    public class EnvResponseWriter : IEnvResponseWriter
    {
        private readonly MetricsOptions _metricsOptions;

        public EnvResponseWriter(
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IOptions<MetricsAspNetCoreOptions> middlewareOptionsAccessor)
        {
            if (metricsOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(metricsOptionsAccessor));
            }

            if (middlewareOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(middlewareOptionsAccessor));
            }

            _metricsOptions = metricsOptionsAccessor.Value;
        }

        public Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default(CancellationToken))
        {
            var formatter = context.Request.GetTypedHeaders().ResolveFormatter(
                _metricsOptions.DefaultOutputEnvFormatter,
                metricsMediaTypeValue => _metricsOptions.OutputEnvFormatters.GetType(metricsMediaTypeValue));

            context.SetNoCacheHeaders();

            if (formatter == default(IEnvOutputFormatter))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                context.Response.Headers[HeaderNames.ContentType] = new[] { context.Request.ContentType };
                return Task.CompletedTask;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.Headers[HeaderNames.ContentType] = new[] { formatter.MediaType.ContentType };

            return formatter.WriteAsync(context.Response.Body, environmentInfo, token);
        }
    }
}