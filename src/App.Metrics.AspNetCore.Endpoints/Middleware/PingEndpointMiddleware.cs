// <copyright file="PingEndpointMiddleware.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Endpoints.Middleware
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class PingEndpointMiddleware
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private readonly ILogger<PingEndpointMiddleware> _logger;

        public PingEndpointMiddleware(
            RequestDelegate next,
            ILogger<PingEndpointMiddleware> logger)
        {
            _logger = logger;
        }

        // ReSharper disable UnusedMember.Global
        public async Task Invoke(HttpContext context)
            // ReSharper restore UnusedMember.Global
        {
            _logger.MiddlewareExecuting<PingEndpointMiddleware>();

            context.Response.Headers["Content-Type"] = new[] { "text/plain" };
            context.SetNoCacheHeaders();

            context.Response.StatusCode = StatusCodes.Status200OK;

            await context.Response.WriteAsync("pong");

            _logger.MiddlewareExecuted<PingEndpointMiddleware>();
        }
    }
}