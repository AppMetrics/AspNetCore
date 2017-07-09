﻿// <copyright file="AsciiMetricsTextResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core.Formatting;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Middleware;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore.Formatters.Ascii
{
    public class AsciiMetricsTextResponseWriter : IMetricsTextResponseWriter
    {
        /// <inheritdoc />
        public string ContentType => "text/plain; app.metrics=vnd.app.metrics.v1.metrics;";

        public Task WriteAsync(HttpContext context, MetricsDataValueSource metricsData, CancellationToken token = default(CancellationToken))
        {
            var payloadBuilder = new AsciiMetricPayloadBuilder();

            var builder = new MetricDataValueSourceFormatter();
            builder.Build(metricsData, payloadBuilder);

            return context.Response.WriteAsync(payloadBuilder.PayloadFormatted(), token);
        }
    }
}