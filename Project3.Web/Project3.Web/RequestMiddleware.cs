using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Project3.Web
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration _configuration;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
            _configuration = Startup.Configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            //var authCookie = CheckAuthCookie(context);

            //if (!authCookie)
            //{
            //    RedirectToLoginPage(context);
            //}
            //else
            //{
            //    RefreshAccessToken(context);
                BeginInvoke(context);
                await _next.Invoke(context);
                EndInvoke(context);
            //}
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

        public void RefreshAccessToken(HttpContext context)
        {
            var request = context.Request;

            try
            {
                var existingRefreshToken = Utilities.Utilities.GetCookieValue(request, "RefreshToken");

                if (!string.IsNullOrEmpty(existingRefreshToken))
                {
                    var buffer = Encoding.ASCII.GetBytes($"grant_type=refresh_token&refresh_token={existingRefreshToken}");

                    var req = (HttpWebRequest)WebRequest.Create($"{_configuration["AppSettings:APIUrl"]}Oauth/Token");
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
                    }
                }
                else
                {
                    RedirectToLoginPage(context);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                RedirectToLoginPage(context);
            }
        }

        public void RedirectToLoginPage(HttpContext context)
        {
            try
            {
                var rd = context.GetRouteData();

                if (rd == null) return;

                var controllerName = rd.Values["controller"].ToString();
                var actionName = rd.Values["action"].ToString();

                if (controllerName == "Account" || actionName == "Login") return;

                context.Response.Redirect("~/Account/Login", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
