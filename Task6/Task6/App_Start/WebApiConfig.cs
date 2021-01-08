using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Task6
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            StripeConfiguration.SetApiKey("sk_test_JYGw8QqkXc05Tz15ZNAur9lK00ErStfDC0");


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
