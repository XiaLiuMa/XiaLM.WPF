using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System.Linq;
using System.Web.Http;

namespace XiaLM.Owin
{
    public class OwinRealize
    {
        private string url;
        private StartOptions startOpts;

        public OwinRealize(string url, OwinStart owinStart)
        {
            this.url = url;
            Startup.owinStart = owinStart;
        }

        public void Start()
        {
            startOpts = new StartOptions(url);
            WebApp.Start<Startup>(startOpts);
        }
    }

    public class Startup
    {
        public static OwinStart owinStart { get; set; }

        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            if (owinStart == null)
            {
                owinStart = new OwinStart();
                owinStart.IsCorss = false;
                owinStart.Routes = null;
                owinStart.IsOpenSignalR = false;
                owinStart.FileServerOptions = null;
                owinStart.IsOpenWebApi = false;
            }
            var routes = owinStart.Routes;
            if (owinStart.IsCorss)
            {
                appBuilder.UseCors(CorsOptions.AllowAll);
                config.EnableCors();
            }
            if (routes == null || routes.Count() <= 0)
            {
                var httpRoute = new HttpRoute();
                config.Routes.MapHttpRoute(
                    name: httpRoute.Name,
                    routeTemplate: httpRoute.RouteTemplate,
                    defaults: httpRoute.Defaults
                );
            }
            else
            {
                foreach (var item in routes)
                {
                    config.Routes.MapHttpRoute(
                    name: item.Name,
                    routeTemplate: item.RouteTemplate,
                    defaults: item.Defaults
                );
                }
            }
            if (owinStart.FileServerOptions != null)
            {
                appBuilder.UseFileServer(owinStart.FileServerOptions.Options);
            }
            if (owinStart.IsOpenSignalR)
            {
                appBuilder.MapSignalR(new HubConfiguration()
                {
                    EnableJSONP = owinStart.ResultToJson
                });
            }
            if (owinStart.ResultToJson)
            {
                config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
                Newtonsoft.Json.JsonSerializerSettings jSettings = new Newtonsoft.Json.JsonSerializerSettings();
                config.Formatters.JsonFormatter.SerializerSettings = jSettings;
                config.Formatters.JsonFormatter.MediaTypeMappings.Insert(0, new System.Net.Http.Formatting.QueryStringMapping("json", "true", "application/json"));
            }
            if (owinStart.IsOpenWebApi)
            {
                appBuilder.UseWebApi(config);
            }
        }
    }
}
