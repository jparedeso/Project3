using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Project3.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Project3.Web.Utilities
{
    public class Utilities
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string ApiUrl;
        public string ApiUrlFull;

        public Utilities(AppSettings appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings;
            _httpContextAccessor = httpContextAccessor;

            ApiUrl = appSettings.APIUrl;
            ApiUrlFull = $"{ApiUrl}api/";
        }

        public static void AddCookie(HttpContext context, string name, string value, DateTime? expires = null)
        {
            var options = new CookieOptions();

            if (expires.HasValue)
            {
                options.Expires = expires.Value;
            }

            context.Response.Cookies.Append(name, Convert.ToBase64String(Encoding.Unicode.GetBytes(value)), options);
        }

        public static void RemoveCookie(HttpContext context, string name)
        {
            if (context.Request.Cookies[name] == null) return;

            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            context.Response.Cookies.Append(name, "", options);

            context.Response.Cookies.Delete(name);
        }

        public static string GetCookieValue(HttpRequest request, string name)
        {
            var cookie = request.Cookies[name];
            if (cookie == null)
            {
                return "";
            }

            var cookieValue = Encoding.Unicode.GetString(Convert.FromBase64String(cookie));
            return cookieValue;
        }

        public static string FormatDateForUrl(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("o");
        }

        public static string FormatDateForUrl(DateTime dateTime)
        {
            return dateTime.ToString("o");
        }
    }
}