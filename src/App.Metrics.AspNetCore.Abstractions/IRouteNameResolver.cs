// <copyright file="IRouteNameResolver.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace App.Metrics.AspNetCore
{
    public interface IRouteNameResolver
    {
        Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData);
    }
}