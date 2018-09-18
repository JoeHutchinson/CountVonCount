using CountVonCount.Infrastructure;
using System.Web.Http;
using TinyIoC;

namespace CountVonCount
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = TinyIoCContainer.Current;

            // Register context, unit of work and repos with per request lifetime
            container.Register<IHtmlProvider, HtmlProvider>().AsSingleton();
            container.Register<ICollect, HtmlCollector>().AsSingleton();
            container.Register<IStorage, SQLDBStore>().AsSingleton();

            // Set Web API dep resolver
            config.DependencyResolver = new TinyIocWebApiDependencyResolver(container);

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
