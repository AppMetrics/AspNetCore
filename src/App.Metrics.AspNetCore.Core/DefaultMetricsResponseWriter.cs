// <copyright file="DefaultMetricsResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace App.Metrics.AspNetCore
{
    public class DefaultMetricsResponseWriter : IMetricsResponseWriter
    {
        private readonly IMetricsOutputFormatter _formatter;
        private readonly MetricsOptions _metricsOptions;

        public DefaultMetricsResponseWriter(
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IOptions<MetricsAspNetCoreOptions> middlewareOptionsAccessor,
            IMetricsOutputFormatter formatter)
        {
            if (middlewareOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(middlewareOptionsAccessor));
            }

            _formatter = formatter;
            _metricsOptions = metricsOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(metricsOptionsAccessor));
        }

        public DefaultMetricsResponseWriter(
            IOptions<MetricsOptions> metricsOptionsAccessor,
            IOptions<MetricsAspNetCoreOptions> middlewareOptionsAccessor)
        {
            if (middlewareOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(middlewareOptionsAccessor));
            }

            _metricsOptions = metricsOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(metricsOptionsAccessor));
        }

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            var formatter = _formatter ?? context.Request.GetTypedHeaders().ResolveFormatter(
                _metricsOptions.DefaultOutputMetricsFormatter,
                metricsMediaTypeValue => _metricsOptions.OutputMetricsFormatters.GetType(metricsMediaTypeValue));

            context.SetNoCacheHeaders();

            if (formatter == default(IMetricsOutputFormatter))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                context.Response.Headers[HeaderNames.ContentType] = new[] { context.Request.ContentType };
                return Task.CompletedTask;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.Headers[HeaderNames.ContentType] = new[] { formatter.MediaType.ContentType };

            return formatter.WriteAsync(context.Response.Body, metricsData, token);
        }
    }
}