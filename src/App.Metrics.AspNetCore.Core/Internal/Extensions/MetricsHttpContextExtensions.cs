// <copyright file="MetricsHttpContextExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Http
    // ReSharper restore CheckNamespace
{
    internal static class MetricsHttpContextExtensions
    {
        private static readonly string MetricsCurrentRouteName = "__App.Metrics.CurrentRouteName__";

        public static void AddMetricsCurrentRouteName(this HttpContext context, string metricName)
        {
            context.Items.Add(MetricsCurrentRouteName, metricName);
        }

        public static string GetMetricsCurrentRouteName(this HttpContext context)
        {
            var route = context.Items[MetricsCurrentRouteName] as string;

            if (route.IsPresent())
            {
                return context.Request.Method + " " + context.Items[MetricsCurrentRouteName];
            }

            return context.Request.Method;
        }

        public static bool HasMetricsCurrentRouteName(this HttpContext context) { return context.Items.ContainsKey(MetricsCurrentRouteName); }
    }
}