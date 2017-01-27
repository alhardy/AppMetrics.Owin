// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable CheckNamespace
namespace System.Web
    // ReSharper restore CheckNamespace
{
    public class MetricsMvcHttpHandler : IHttpHandler
    {
        public MetricsMvcHttpHandler(RequestContext requestContext)
        {
            RequestContext = requestContext;
        }

        public bool IsReusable => true;

        public RequestContext RequestContext { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            var controller = RequestContext.RouteData.GetRequiredString("controller");
            IController controllerInstance = null;
            IControllerFactory factory = null;

            try
            {
                factory = ControllerBuilder.Current.GetControllerFactory();
                controllerInstance = factory.CreateController(RequestContext, controller);

                var controllerName = RequestContext.RouteData.Values["controller"] as string;
                var actionName = RequestContext.RouteData.Values["action"] as string;

                context.GetOwinContext().Environment.Add("__App.Metrics.CurrentRouteName__",
                    $"{controllerName?.ToLowerInvariant()}/{actionName?.ToLowerInvariant()}");

                controllerInstance?.Execute(RequestContext);
            }
            finally
            {
                factory?.ReleaseController(controllerInstance);
            }
        }
    }
}