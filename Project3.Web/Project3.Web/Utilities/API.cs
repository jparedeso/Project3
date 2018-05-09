using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Project3.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project3.Web.Utilities
{
    public class API
    {
        private static HttpClient _createClient(AppSettings appSettings, IHttpContextAccessor httpContextAccessor)
        {
            var utilities = new Utilities(appSettings, httpContextAccessor);
            var client = new HttpClient
            {
                BaseAddress = new Uri(utilities.ApiUrlFull)
            };

            string accessToken = Utilities.GetCookieValue(httpContextAccessor.HttpContext.Request, "AccessToken");
            //if (string.IsNullOrWhiteSpace(accessToken))
            //{
            //    throw new HttpRequestException("Unatuhorized");
            //}

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static async Task<TContent> Get<TContent>(AppSettings appSettings, IHttpContextAccessor httpContextAccessor, string requestString) where TContent : class
        {
            using (var client = _createClient(appSettings, httpContextAccessor))
            {
                Debug.WriteLine(string.Format("WebAPI ASYNC REQ: GET {0}", requestString));
                var requestTask = client.GetAsync(requestString);
                return await GetResponseContent<TContent>(requestTask).ConfigureAwait(false);
            }
        }

        public static async Task<HttpResponseMessage> Post<TContent>(AppSettings appSettings, IHttpContextAccessor httpContextAccessor, string requestString, TContent content)
        {
            using (var client = _createClient(appSettings, httpContextAccessor))
            {
                Debug.WriteLine("WebAPI ASYNC REQ: POST {0}, CONTENT TYPE: {1}", requestString, typeof(TContent).FullName);
                var json = JsonConvert.SerializeObject(content);
                var result = await client.PostAsync(requestString, new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    var errorContent = await result.Content.ReadAsStringAsync();
                    Debug.WriteLine("WebAPI ASYNC RESP: {0}: {1}, ERROR: ", result.StatusCode, result.ReasonPhrase);
                    throw new HttpRequestException(errorContent);
                }
                return result;
            }
        }

        public static async Task<HttpResponseMessage> Put<TContent>(AppSettings appSettings, IHttpContextAccessor httpContextAccessor, string requestString, TContent content)
        {
            using (HttpClient client = _createClient(appSettings, httpContextAccessor))
            {
                Debug.WriteLine("WebAPI ASYNC REQ: PUT {0}, CONTENT TYPE: {1}", requestString, typeof(TContent).FullName);
                var json = JsonConvert.SerializeObject(content);
                var result = await client.PutAsync(requestString, new StringContent(json, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    var errorContent = await result.Content.ReadAsStringAsync();
                    Debug.WriteLine("WebAPI ASYNC RESP: {0}: {1}, ERROR: ", result.StatusCode, result.ReasonPhrase);
                    throw new HttpRequestException(errorContent);
                }
                return result;
            }
        }

        public static async Task<TContent> GetResponseContent<TContent>(Task<HttpResponseMessage> responseTask) where TContent : class
        {
            var response = await responseTask.ConfigureAwait(false);
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("API ASYNC RESP: {0}: {1}", response.StatusCode, response.ReasonPhrase);
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                }

                var settings = new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
                };


                var str = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("API ASYNC RESP CODE: {0}, CONTENT TYPE: <{1}>", response.StatusCode, typeof(TContent).FullName);

                return JsonConvert.DeserializeObject<TContent>(str, settings);
            }
            catch (AggregateException aex)
            {
                aex.Flatten();
                Debug.WriteLine(aex.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}