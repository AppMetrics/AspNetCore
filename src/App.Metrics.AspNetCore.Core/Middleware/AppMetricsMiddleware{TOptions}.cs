﻿// <copyright file="AppMetricsMiddleware{TOptions}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Middleware
{
    public abstract class AppMetricsMiddleware<TOptions>
        where TOptions : MetricsAspNetCoreOptions, new()
    {
        private readonly Func<PathString, bool> _shouldRecordMetric;

        protected AppMetricsMiddleware(
            RequestDelegate next,
            IOptions<TOptions> metricsAspNetCoreOptionsAccessor,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
        {
            Options = metricsAspNetCoreOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(metricsAspNetCoreOptionsAccessor));
            Logger = loggerFactory?.CreateLogger(GetType().FullName) ?? throw new ArgumentNullException(nameof(loggerFactory));
            Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            Next = next ?? throw new ArgumentNullException(nameof(next));

            IReadOnlyList<Regex> ignoredRoutes = Options.IgnoredRoutesRegexPatterns.
                                                         Select(p => new Regex(p, RegexOptions.Compiled | RegexOptions.IgnoreCase)).
                                                         ToList();

            if (ignoredRoutes.Any())
            {
                _shouldRecordMetric = path => !ignoredRoutes.Any(
                    ignorePattern => ignorePattern.IsMatch(
                        path.ToString().RemoveLeadingSlash()));
            }
            else
            {
                _shouldRecordMetric = path => true;
            }
        }

        protected ILogger Logger { get; }

        protected IMetrics Metrics { get; }

        protected RequestDelegate Next { get; }

        protected TOptions Options { get; }

        protected string GetOAuthClientIdIfRequired(HttpContext context) { return Options.OAuth2TrackingEnabled ? context.OAuthClientId() : null; }

        protected bool PerformMetric(HttpContext context)
        {
            if (Options.IgnoredRoutesRegexPatterns == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(context.Request.Path))
            {
                return false;
            }

            return _shouldRecordMetric(context.Request.Path);
        }

        protected bool ShouldTrackHttpStatusCode(int httpStatusCode) { return Options.IgnoredHttpStatusCodes.All(i => i != httpStatusCode); }

        protected Task WriteResponseAsync(
            HttpContext context,
            string content,
            string contentType,
            HttpStatusCode code = HttpStatusCode.OK,
            string warning = null)
        {
            context.Response.Headers["Content-Type"] = new[] { contentType };
            context.SetNoCacheHeaders();

            if (warning.IsPresent())
            {
                context.Response.Headers["Warning"] = new[] { $"Warning: 100 '{warning}'" };
            }

            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(content);
        }
    }
}