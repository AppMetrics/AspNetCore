// <copyright file="ToleratingController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using MetricsSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace MetricsSandboxMvc.Controllers
{
    [Route("api/[controller]")]
    public class ToleratingController : ControllerBase
    {
        private readonly RequestDurationForApdexTesting _durationForApdexTesting;

        public ToleratingController(RequestDurationForApdexTesting durationForApdexTesting)
        {
            _durationForApdexTesting = durationForApdexTesting;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            var duration = _durationForApdexTesting.NextToleratingDuration;

            await Task.Delay(duration, HttpContext.RequestAborted);

            return duration;
        }
    }
}