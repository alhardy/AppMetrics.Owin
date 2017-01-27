// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Routing;

namespace App.Metrics.Extensions.Owin.WebApi
{
    public class MetricsWebApiMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var owinContext = request.GetOwinContext();

            if (owinContext == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var routes = request.GetConfiguration().Routes;

            if (routes == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var routeData = routes.GetRouteData(request);

            if (routeData == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var subRoutes = routeData.Values["MS_SubRoutes"] as IHttpRouteData[];

            if (subRoutes == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var routeTemplate = subRoutes[0].Route.RouteTemplate;

            owinContext.Environment.Add("__App.Metrics.CurrentRouteName__", routeTemplate?.ToLowerInvariant());

            return base.SendAsync(request, cancellationToken);
        }
    }
}