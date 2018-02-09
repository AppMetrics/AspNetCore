// <copyright file="MvcOptionsExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc
    // ReSharper restore CheckNamespace
{
    public static class MvcOptionsExtensions
    {
        [Obsolete("Use IMvcBuilder.AddMvc()")]
        public static MvcOptions AddMetricsResourceFilter(this MvcOptions options)
        {
            if (!options.Filters.OfType<MetricsResourceFilter>().Any())
            {
                options.Filters.Add(new MetricsResourceFilter(new MvcRouteTemplateResolver()));
            }

            return options;
        }

        [Obsolete("Use IMvcBuilder.AddMvc()")]
        public static MvcOptions AddMetricsResourceFilter(this MvcOptions options, IRouteNameResolver routeNameResolver)
        {
            if (!options.Filters.OfType<MetricsResourceFilter>().Any())
            {
                options.Filters.Add(new MetricsResourceFilter(routeNameResolver));
            }

            return options;
        }
    }
}