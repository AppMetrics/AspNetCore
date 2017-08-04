// <copyright file="RequestHeaderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;
using App.Metrics.Formatters;
using Microsoft.Net.Http.Headers;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http.Headers
    // ReSharper restore CheckNamespace
{
    public static class RequestHeaderExtensions
    {
        public static(TFormatter Formatter, Encoding Encoding) ResolveFormatter<TFormatter>(
            this RequestHeaders headers,
            TFormatter defaultFormatter,
            Func<MetricsMediaTypeValue, TFormatter> resolveOutputFormatter)
        {
            if (headers.Accept == null)
            {
                return (defaultFormatter, Encoding.Default);
            }

            var formatter = defaultFormatter;
            var encoding = Encoding.Default;

            foreach (var accept in headers.Accept)
            {
                var metricsMediaTypeValue = accept.ToMetricsMediaType();

                if (metricsMediaTypeValue != default(MetricsMediaTypeValue))
                {
                    formatter = resolveOutputFormatter(metricsMediaTypeValue);
                }

                if (formatter != null)
                {
                    encoding = accept.Encoding ?? encoding;

                    return (formatter, encoding);
                }
            }

            return (defaultFormatter, Encoding.Default);
        }
    }
}
