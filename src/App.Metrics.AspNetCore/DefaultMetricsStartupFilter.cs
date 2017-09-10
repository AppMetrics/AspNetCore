// <copyright file="DefaultMetricsStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace App.Metrics.AspNetCore
{
    /// <summary>
    /// Inserts the App Metrics Middleware at the request pipeline
    /// </summary>
    public class DefaultMetricsStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddAllMetricsEndpointsAndTrackingMiddleware;

            void AddAllMetricsEndpointsAndTrackingMiddleware(IApplicationBuilder app)
            {
                app.UseMetricsAllEndpoints();
                app.UseMetricsAllMiddleware();

                next(app);
            }
        }
    }
}