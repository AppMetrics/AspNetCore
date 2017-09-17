// <copyright file="FrustratingController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using MetricsReportingSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Mvc;

namespace MetricsReportingSandboxMvc.Controllers
{
    [Route("api/[controller]")]
    public class FrustratingController : Controller
    {
        private readonly RequestDurationForApdexTesting _durationForApdexTesting;

        public FrustratingController(RequestDurationForApdexTesting durationForApdexTesting)
        {
            _durationForApdexTesting = durationForApdexTesting;
        }

        [HttpGet]
        public async Task<int> Get()
        {
            var duration = _durationForApdexTesting.NextFrustratingDuration;

            await Task.Delay(duration, HttpContext.RequestAborted);

            return duration;
        }
    }
}