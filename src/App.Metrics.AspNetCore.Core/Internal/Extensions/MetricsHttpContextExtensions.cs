// <copyright file="MetricsHttpContextExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
    // ReSharper restore CheckNamespace
{
    public static class MetricsHttpContextExtensions
    {
        private static readonly string MetricsCurrentRouteName = "__App.Metrics.CurrentRouteName__";

        public static void AddMetricsCurrentRouteName(this HttpContext context, string metricName)
        {
            if (!context.Items.ContainsKey(MetricsCurrentRouteName))
            {
                context.Items.Add(MetricsCurrentRouteName, metricName);
            }
        }

        public static string GetMetricsCurrentRouteName(this HttpContext context)
        {
            if (context.Items.TryGetValue(MetricsCurrentRouteName, out var item))
            {
                var route = item as string;
                if (route.IsPresent())
                {
                    return $"{context.Request.Method} {route}";
                }
            }

            // TODO: #25 MvcRouteTemplateResolver isn't executed when an unsupported api version is requested
            // leaving thier metrics tagged with the http method alone.
            // The raw api version requested can be retrieved from MS_ApiVersionRequestProperties on HttpContext.Items

            return context.Request.Method;
        }

        public static bool HasMetricsCurrentRouteName(this HttpContext context) { return context.Items.ContainsKey(MetricsCurrentRouteName); }
    }
}