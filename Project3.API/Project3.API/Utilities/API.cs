using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Project3.API.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Project3.API.Utilities
{
    public class API
    {
        private static HttpClient _createClient(string baseAddress, Dictionary<string, string> kv)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress ?? "https://api-endpoint.igdb.com/")
            };
            //var client = new HttpClient();
            //client.BaseAddress = new Uri(baseAddress ?? "https://api-endpoint.igdb.com/");

            if (kv == null)
            {
                kv = new Dictionary<string, string> {{"user-key", "126f36bc58936fd431718ef9baaabf11"}};
            }

            client.DefaultRequestHeaders.Add(kv.First().Key, kv.First().Value);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static async Task<TContent> Get<TContent>(string requestString) where TContent : class
        {
            return await Get<TContent>(requestString, null);
        }

        public static async Task<TContent> Get<TContent>(string requestString, string baseAddress) where TContent : class
        {
            return await Get<TContent>(requestString, baseAddress, null);
        }

        public static async Task<TContent> Get<TContent>(string requestString, string baseAddress, Dictionary<string, string> kv) where TContent : class
        {
            using (var client = _createClient(baseAddress, kv))
            {
                Debug.WriteLine(string.Format("API ASYNC REQ: GET {0}", requestString));
                var requestTask = client.GetAsync(requestString);
                return await GetResponseContent<TContent>(requestTask).ConfigureAwait(false);
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