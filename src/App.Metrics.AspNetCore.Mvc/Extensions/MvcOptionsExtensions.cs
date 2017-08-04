// <copyright file="MvcOptionsExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc
    // ReSharper restore CheckNamespace
{
    public static class MvcOptionsExtensions
    {
        public static MvcOptions AddMetricsResourceFilter(this MvcOptions options)
        {
            options.Filters.Add(new MetricsResourceFilter(new MvcRouteTemplateResolver()));

            return options;
        }

        public static MvcOptions AddMetricsResourceFilter(this MvcOptions options, IRouteNameResolver routeNameResolver)
        {
            options.Filters.Add(new MetricsResourceFilter(routeNameResolver));

            return options;
        }
    }
}