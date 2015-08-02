using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Web.Http;
using Newtonsoft.Json;

namespace SharedPlaylistApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                           name: "ActionApi",
                           routeTemplate: "api/{controller}/{action}/{id}",
                           defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
                           );


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );





            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();


            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                MaxDepth = 2,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };

            config.Formatters.JsonFormatter.SerializerSettings = serializerSettings;

        }
    }
}
