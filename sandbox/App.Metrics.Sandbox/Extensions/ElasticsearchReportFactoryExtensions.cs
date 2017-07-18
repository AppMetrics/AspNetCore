// <copyright file="ElasticsearchReportFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Reporting
    // ReSharper restore CheckNamespace
{
    public static class ElasticsearchReportFactoryExtensions
    {
        // private static readonly string ElasticSearchIndex = "appmetricssandbox";
        // private static readonly Uri ElasticSearchUri = new Uri("http://127.0.0.1:9200");

        public static void AddElasticSearchReporting(this IReportFactory factory)
        {
            // factory.AddElasticSearch(
            //     new ElasticSearchReporterSettings
            //     {
            //         HttpPolicy = new HttpPolicy
            //         {
            //             FailuresBeforeBackoff = 3,
            //             BackoffPeriod = TimeSpan.FromSeconds(30),
            //             Timeout = TimeSpan.FromSeconds(10)
            //         },
            //         ElasticSearchSettings = new ElasticSearchSettings(ElasticSearchUri, ElasticSearchIndex),
            //         ReportInterval = TimeSpan.FromSeconds(5)
            //     });
        }
    }
}
