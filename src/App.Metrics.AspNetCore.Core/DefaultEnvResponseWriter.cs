// <copyright file="DefaultEnvResponseWriter.cs" company="Allan Hardy">
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
    public class DefaultEnvResponseWriter : IEnvResponseWriter
    {
        private readonly IEnvOutputFormatter _formatter;
        private readonly MetricsOptions _metricsOptions;

        public DefaultEnvResponseWriter(
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IEnvOutputFormatter formatter)
        {
            if (metricsOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(metricsOptionsAccessor));
            }

            _formatter = formatter;
            _metricsOptions = metricsOptionsAccessor.Value;
        }

        public DefaultEnvResponseWriter(IOptions<MetricsOptions> metricsOptionsAccessor)
        {
            if (metricsOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(metricsOptionsAccessor));
            }

            _metricsOptions = metricsOptionsAccessor.Value;
        }

        public Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default(CancellationToken))
        {
            var formatter = _formatter ?? context.Request.GetTypedHeaders().ResolveFormatter(
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