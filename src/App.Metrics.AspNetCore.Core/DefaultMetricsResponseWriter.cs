﻿// <copyright file="DefaultMetricsResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace App.Metrics.AspNetCore
{
    public class DefaultMetricsResponseWriter : IMetricsResponseWriter
    {
        private readonly IMetricsOutputFormatter _fallbackFormatter;
        private readonly IMetricsOutputFormatter _formatter;
        private readonly MetricsFormatterCollection _formatters;

        public DefaultMetricsResponseWriter(
            IMetricsOutputFormatter fallbackFormatter,
            IReadOnlyCollection<IMetricsOutputFormatter> formatters)
        {
            if (formatters == null)
            {
                throw new ArgumentNullException(nameof(formatters));
            }

            // TODO: Need to do this?
            _formatters = new MetricsFormatterCollection(formatters.ToList());
            _fallbackFormatter = fallbackFormatter;
        }

        public DefaultMetricsResponseWriter(IMetricsOutputFormatter formatter)
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        /// <inheritdoc />
        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default)
        {
            var formatter = _formatter ?? context.Request.GetTypedHeaders().ResolveFormatter(
                                _fallbackFormatter,
                                metricsMediaTypeValue => _formatters.GetType(metricsMediaTypeValue));

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