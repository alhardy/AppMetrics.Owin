// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Extensions.Owin.Extensions;
using App.Metrics.Extensions.Owin.Internal;
using App.Metrics.Gauge;
using App.Metrics.Tagging;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    internal static class MetricsExtensions
    {
        public static IMetrics DecrementActiveRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Decrement(OwinMetricsRegistry.Contexts.HttpRequests.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetrics ErrorRequestPercentage(this IMetrics metrics)
        {
            var errors = metrics.Provider.Meter.Instance(OwinMetricsRegistry.Contexts.HttpRequests.Meters.HttpErrorRequests);
            var requests = metrics.Provider.Timer.Instance(OwinMetricsRegistry.Contexts.HttpRequests.Timers.WebRequestTimer);

            metrics.Measure.Gauge.SetValue(
                OwinMetricsRegistry.Contexts.HttpRequests.Gauges.PercentageErrorRequests,
                () => new HitPercentageGauge(errors, requests, m => m.OneMinuteRate));

            return metrics;
        }

        public static IMetrics IncrementActiveRequests(this IMetrics metrics)
        {
            metrics.Measure.Counter.Increment(OwinMetricsRegistry.Contexts.HttpRequests.Counters.ActiveRequests);

            return metrics;
        }

        public static IMetrics MarkHttpEndpointForOAuthClient(this IMetrics metrics, string routeTemplate, string clientId, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                OwinMetricsRegistry.Contexts.OAuth2.Meters.EndpointHttpRequests(routeTemplate),
                new MetricSetItem("http_status_code", httpStatusCode.ToString()));

            return metrics;
        }

        public static IMetrics MarkHttpRequestEndpointError(this IMetrics metrics, string routeTemplate, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                OwinMetricsRegistry.Contexts.HttpRequests.Meters.EndpointHttpErrorRequests(routeTemplate),
                new MetricSetItem("http_status_code", httpStatusCode.ToString()));

            return metrics;
        }

        public static IMetrics MarkHttpRequestError(this IMetrics metrics, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                OwinMetricsRegistry.Contexts.HttpRequests.Meters.HttpErrorRequests,
                new MetricSetItem("http_status_code", httpStatusCode.ToString()));

            return metrics;
        }

        public static IMetrics MarkHttpRequestForOAuthClient(this IMetrics metrics, string clientId, int httpStatusCode)
        {
            metrics.Measure.Meter.Mark(
                OwinMetricsRegistry.Contexts.OAuth2.Meters.HttpRequests,
                new MetricSetItem(new[] { "client_id", "http_status_code" }, new[] { clientId, httpStatusCode.ToString() }));

            return metrics;
        }

        public static IMetrics RecordEndpointRequestTime(this IMetrics metrics, string clientId, string routeTemplate, long elapsed)
        {
            metrics.Provider.Timer
                   .Instance(OwinMetricsRegistry.Contexts.HttpRequests.Timers.EndpointPerRequestTimer(routeTemplate))
                   .Record(elapsed, TimeUnit.Nanoseconds, clientId.IsPresent() ? clientId : null);

            return metrics;
        }

        public static IMetrics UpdatePostAndPutRequestSize(this IMetrics metrics, long value)
        {
            metrics.Measure.Histogram.Update(OwinMetricsRegistry.Contexts.HttpRequests.Histograms.PostAndPutRequestSize, value);
            return metrics;
        }
    }
}