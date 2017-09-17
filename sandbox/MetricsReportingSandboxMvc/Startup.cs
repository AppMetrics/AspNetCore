// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsReportingSandboxMvc
{
    public class Startup
    {
        private const bool HaveAppRunSampleRequests = true;

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            app.UseTestStuff(lifetime, HaveAppRunSampleRequests);

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTestStuff();

            // DEVNOTE: To add reporting on the IServiceCollection directly as opposed to use the IWebHostBuilder
            // services.AddMetricsReportingCore()
            //     .AddConsole()
            //     .AddTextFile(
            //         textFileOptions =>
            //         {
            //             textFileOptions.OutputPathAndFileName = @"C:\metrics\metrics_web.txt";
            //             textFileOptions.ReportInterval = TimeSpan.FromSeconds(20);
            //             textFileOptions.AppendMetricsToTextFile = false;
            //         })
            //     .AddHostedServiceSchedulingCore();

            services.AddMvc(options => options.AddMetricsResourceFilter());
        }
    }
}