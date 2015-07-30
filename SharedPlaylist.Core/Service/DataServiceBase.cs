using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SharedPlaylist.Core.Models;
using SpotifyAPI.Web.Models;

namespace SharedPlaylist.Core.Service
{
    public abstract class DataServiceBase
    {
        protected const string CONTENT_TYPE_FORM_URLENCODED = "application/x-www-form-urlencoded; charset=utf-8";
        protected const string CONTENT_TYPE_APPLICATION_JSON = "application/json; charset=utf-8";
        protected const string CONTENT_TYPE_APPLICATION_XML = "application/xml; charset=utf-8";
        protected const string CONTENT_TYPE_TEXT_XML = "application/atom+xml";

        public Token UserToken;

        protected bool UseGzipCompression = true;
        protected TimeSpan DefaultTimeout = TimeSpan.FromSeconds(20);
        protected Dictionary<String, String> DefaultHeaders = new Dictionary<string, string>();

        protected async Task<HttpResponse<T>> RequestAsync<T>(string url, HttpMethod method, string content = null, string contentType = null, TimeSpan? timeout = null)
        {
            // #1 Create HttpRequestMessage
            var requestMessage = new HttpRequestMessage();
            requestMessage.Method = method;
            requestMessage.Content = CreateHttpContent(content, contentType);

            requestMessage.RequestUri = new Uri(url, UriKind.Absolute);


            if (UserToken != null)
            {
                requestMessage.Headers.Add("Authorization", String.Format("Bearer {0}", UserToken.AccessToken));
            }

            // #2 Create HttpResponse
            var response = new HttpResponse<T>();
            Debug.WriteLine(url);

            // #3 Create HttpClient
            var client = CreateHttpClient(timeout);

            // #4 Send Request and process response
            try
            {
                var result = await client.SendAsync(requestMessage);
                response.HttpStatusCode = result.StatusCode;
                Debug.WriteLine("Status: " + response.HttpStatusCode);

                if (result.IsSuccessStatusCode)
                {
                    if (typeof(T) == typeof(String))
                    {
                        response.Content = (T)(object)await result.Content.ReadAsStringAsync();
                    }
                    else if (typeof(T) == typeof(byte[]))
                    {
                        response.Content = (T)(object)await result.Content.ReadAsByteArrayAsync();
                    }
                }
                if (response.HttpStatusCode == HttpStatusCode.InternalServerError)
                {

                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is WebException && ((WebException)ex.InnerException).Response is HttpWebResponse)
                {
                    response.HttpStatusCode = ((HttpWebResponse)((WebException)ex.InnerException).Response).StatusCode;
                    response.Message = ((HttpWebResponse)((WebException)ex.InnerException).Response).StatusDescription;
                }
                else
                {
                    response.HttpStatusCode = HttpStatusCode.BadGateway;
                    response.Message = ex.Message;
                }
            }

            return response;
        }

        private HttpContent CreateHttpContent(string content, string contentType)
        {
            if (!String.IsNullOrEmpty(content))
            {
                var httpContent = new StringContent(content);
                if (!String.IsNullOrEmpty(contentType))
                    httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                return httpContent;
            }

            return null;
        }

        private HttpClient CreateHttpClient(TimeSpan? timeout = null)
        {
            var handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            if (timeout.HasValue)
                client.Timeout = timeout.Value;
            else
                client.Timeout = DefaultTimeout;

            if (UseGzipCompression && handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip;
                client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            }

            foreach (var item in DefaultHeaders)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);

            }

            return client;
        }
    }
}
