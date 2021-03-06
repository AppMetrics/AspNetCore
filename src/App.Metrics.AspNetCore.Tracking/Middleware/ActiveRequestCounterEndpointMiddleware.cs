﻿// <copyright file="ActiveRequestCounterEndpointMiddleware.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Tracking.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class ActiveRequestCounterEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ActiveRequestCounterEndpointMiddleware> _logger;
        private readonly IMetrics _metrics;

        public ActiveRequestCounterEndpointMiddleware(
            RequestDelegate next,
            ILogger<ActiveRequestCounterEndpointMiddleware> logger,
            IMetrics metrics)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _metrics = metrics;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<ActiveRequestCounterEndpointMiddleware>();

            _metrics.IncrementActiveRequests();

            try
            {
                await _next(context);
                _metrics.DecrementActiveRequests();
            }
            catch (Exception)
            {
                _metrics.DecrementActiveRequests();
                throw;
            }
            finally
            {
                _logger.MiddlewareExecuted<ActiveRequestCounterEndpointMiddleware>();
            }
        }
    }
}