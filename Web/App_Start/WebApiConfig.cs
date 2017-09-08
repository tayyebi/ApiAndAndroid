﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
// using System.Web.Http.Cors;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "mobile/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
