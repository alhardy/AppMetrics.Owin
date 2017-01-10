using System;
using System.Diagnostics;

namespace App.Metrics.Extensions.Owin.Extensions
{
    internal static class StringExtensions
    {
        [DebuggerStepThrough]
        internal static string CleanUrlPath(this string url)
        {
            if (string.IsNullOrWhiteSpace(url)) url = "/";

            if (url != "/" && url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return url;
        }

        [DebuggerStepThrough]
        internal static string EnsureLeadingSlash(this string url)
        {
            if (!url.StartsWith("/"))
            {
                return "/" + url;
            }

            return url;
        }

        [DebuggerStepThrough]
        internal static string EnsureTrailingSlash(this string url)
        {
            if (!url.EndsWith("/"))
            {
                return url + "/";
            }

            return url;
        }

        [DebuggerStepThrough]
        internal static string GetSafeString(Func<string> action)
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        [DebuggerStepThrough]
        internal static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        internal static bool IsPresent(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        internal static string RemoveLeadingSlash(this string url)
        {
            if (url != null && url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            return url;
        }
    }
}