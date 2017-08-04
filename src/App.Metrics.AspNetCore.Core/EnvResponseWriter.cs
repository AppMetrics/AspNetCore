// <copyright file="EnvResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;
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
            var acceptHeaderMediaType = context.Request.GetTypedHeaders();
            var formatter = default(IEnvOutputFormatter);
            var encoding = Encoding.Default;

            context.SetNoCacheHeaders();
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (acceptHeaderMediaType.Accept != null)
            {
                foreach (var accept in acceptHeaderMediaType.Accept)
                {
                    formatter = ResolveFormatter(_metricsOptions.OutputEnvFormatters, accept);

                    if (formatter != default(IEnvOutputFormatter))
                    {
                        encoding = accept.Encoding ?? encoding;
                        break;
                    }
                }
            }

            if (formatter == default(IEnvOutputFormatter))
            {
                if (_metricsOptions.DefaultEnvOutputFormatter == default(IEnvOutputFormatter))
                {
                    context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                    return Task.CompletedTask;
                }

                formatter = _metricsOptions.DefaultEnvOutputFormatter;
            }

            context.Response.Headers[HeaderNames.ContentType] = new[] { formatter.MediaType.ContentType };

            return formatter.WriteAsync(context.Response.Body, environmentInfo, encoding, token);
        }

        private static IEnvOutputFormatter ResolveFormatter(EnvFormatterCollection formatters, MediaTypeHeaderValue acceptHeader)
        {
            var versionAndFormatTokens = acceptHeader.SubType.Value.Split('-');

            if (acceptHeader.Type.Value.IsMissing()
                || acceptHeader.SubType.Value.IsMissing()
                || versionAndFormatTokens.Length != 2)
            {
                return default(IEnvOutputFormatter);
            }

            var versionAndFormat = versionAndFormatTokens[1].Split('+');

            if (versionAndFormat.Length != 2)
            {
                return default(IEnvOutputFormatter);
            }

            var mediaTypeValue = new MetricsMediaTypeValue(
                acceptHeader.Type.Value,
                versionAndFormatTokens[0],
                versionAndFormat[0],
                versionAndFormat[1]);

            return formatters.GetType(mediaTypeValue);
        }
    }
}