using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Project3.Web;
using Project3.Web.Utilities;

namespace Project3.Web
{
    public static class GetRoutesMiddlewareExtensions
    {
        public static IApplicationBuilder UseGetRoutesMiddleware(this IApplicationBuilder app, Action<IRouteBuilder> configureRoutes)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var routes = new RouteBuilder(app)
            {
                DefaultHandler = app.ApplicationServices.GetRequiredService<MvcRouteHandler>(),
            };
            configureRoutes(routes);
            routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(app.ApplicationServices));
            var router = routes.Build();

            return app.UseMiddleware<RequestMiddleware>(router);
        }
    }

    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRouter _router;
        private IConfiguration _configuration;

        public RequestMiddleware(RequestDelegate next, IRouter router)
        {
            _next = next;
            _router = router;
            _configuration = Startup.Configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var authCookie = CheckAuthCookie(context);

            if (!authCookie)
            {
                await RedirectToLoginPage(context);
            }
            else
            {
                await RefreshAccessToken(context);
                BeginInvoke(context);
                await _next.Invoke(context);
                EndInvoke(context);
            }
        }

        private bool CheckAuthCookie(HttpContext context)
        {
            var accessToken = context?.Request.Cookies["AccessToken"];

            return !string.IsNullOrEmpty(accessToken);
        }

        private void BeginInvoke(HttpContext context)
        {
            // Do custom work after controller execution
        }

        private void EndInvoke(HttpContext context)
        {
            // Do custom work after controller execution
        }

        public async Task RefreshAccessToken(HttpContext context)
        {
            var request = context.Request;

            try
            {
                var existingRefreshToken = Utilities.Utilities.GetCookieValue(request, "RefreshToken");

                if (!string.IsNullOrEmpty(existingRefreshToken))
                {
                    var buffer = Encoding.ASCII.GetBytes($"grant_type=refresh_token&refresh_token={existingRefreshToken}");

                    var req = (HttpWebRequest)WebRequest.Create($"{_configuration["AppSettings:APIUrl"]}oauth/Token");
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = buffer.Length;

                    var stream = req.GetRequestStream();
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();

                    using (var response = (HttpWebResponse)req.GetResponse())
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var body = JsonConvert.DeserializeObject<Dictionary<string, object>>(reader.ReadToEnd());
                        var accessToken = body["access_token"].ToString();
                        var expires = Convert.ToDateTime(body["expires"].ToString());

                        Utilities.Utilities.AddCookie(context, "AccessToken", accessToken, expires);
                        Utilities.Utilities.AddCookie(context, "RefreshToken", existingRefreshToken, DateTime.Now + new TimeSpan(14, 0, 0, 0));
                    }
                }
                else
                {
                    await RedirectToLoginPage(context);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await RedirectToLoginPage(context);
            }
        }

        public async Task RedirectToLoginPage(HttpContext context)
        {
            try
            {
                var routerContext = new RouteContext(context);
                routerContext.RouteData.Routers.Add(_router);

                await _router.RouteAsync(routerContext);

                if (routerContext.Handler != null)
                {
                    var rd = routerContext.RouteData;

                    if (rd == null) return;

                    var controllerName = rd.Values["controller"].ToString();
                    var actionName = rd.Values["action"].ToString();

                    if (controllerName == "Home" ||
                        controllerName == "Account" && (actionName == "Login" || actionName == "Register"))
                    {
                        await _next.Invoke(context);
                    }
                    else
                    {
                        context.Response.Redirect("~/Account/Login", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}