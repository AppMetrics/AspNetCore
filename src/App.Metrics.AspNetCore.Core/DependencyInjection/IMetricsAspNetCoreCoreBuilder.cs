// <copyright file="IMetricsAspNetCoreCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Provides extensiblity on App Metrics middleware e.g. serialization options on supported endpoints.
    /// </summary>
    public interface IMetricsAspNetCoreCoreBuilder
    {
        IServiceCollection Services { get; }
    }
}