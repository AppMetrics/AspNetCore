﻿// <copyright file="IEnvResponseWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace App.Metrics.AspNetCore
{
    public interface IEnvResponseWriter
    {
        Task WriteAsync(HttpContext context, EnvironmentInfo environmentInfo, CancellationToken token = default);
    }
}