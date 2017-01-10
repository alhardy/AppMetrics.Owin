// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace App.Metrics.Extensions.Owin.Internal
{
    internal static class Constants
    {
        public static class DefaultRoutePaths
        {
            public const string HealthEndpoint = "/health";
            public const string MetricsEndpoint = "/metrics";
            public const string MetricsTextEndpoint = "/metrics-text";
            public const string PingEndpoint = "/ping";
        }

        public static class ReservoirSampling
        {
            public const int DefaultSampleSize = 1028;
            public const double DefaultExponentialDecayFactor = 0.015;
            public const double DefaultApdexTSeconds = 0.5;
            public const int ApdexRequiredSamplesBeforeCalculating = 100;
        }
    }
}