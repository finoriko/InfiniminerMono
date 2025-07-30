using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InfiniminerServer
{
    public static class HttpRequest
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static string Post(string url, Dictionary<string, string> postData)
        {
            try
            {
                var formContent = new FormUrlEncodedContent(postData);
                var response = httpClient.PostAsync(url, formContent).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string Get(string url, Dictionary<string, string> parameters)
        {
            try
            {
                string queryString = "";
                if (parameters != null && parameters.Count > 0)
                {
                    var queryParams = new List<string>();
                    foreach (var param in parameters)
                    {
                        queryParams.Add($"{param.Key}={Uri.EscapeDataString(param.Value)}");
                    }
                    queryString = "?" + string.Join("&", queryParams);
                }

                var response = httpClient.GetAsync(url + queryString).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}