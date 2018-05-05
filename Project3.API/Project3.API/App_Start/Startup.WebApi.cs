using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Project3.API
{
    public partial class Startup
    {
        public void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}
