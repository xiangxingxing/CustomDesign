using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpOperation
{
    public interface IHttpService
    {
        Task PostResponseAsync<T>(string url, T parameters, string cookie,
            CancellationToken cancellationToken,
            string version = null, HttpStatusCode expectedStatusCode = HttpStatusCode.OK);
    }

    public class HttpService : IHttpService
    {
        private HttpClient Client { get; set; }
        private int TimeoutSeconds { get; }

        public HttpService(int timeoutSeconds)
        {
            TimeoutSeconds = timeoutSeconds;
        }

        public HttpService()
        {
        }

        public async Task PostResponseAsync<T>(string url,
            T parameters, string cookie, CancellationToken cancellationToken,
            string version = null, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var response = await PostResponseAsync(url, parameters, cookie, cancellationToken, version, true,
                expectedStatusCode);
            await response.Content.ReadAsStringAsync();
        }

        private async Task<HttpResponseMessage> PostResponseAsync<T>(string url,
            T parameters, string cookie, CancellationToken cancellationToken,
            string version = null, bool useApplicationJson = false,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            version = !string.IsNullOrWhiteSpace(version) ? $";v={version}" : string.Empty;
            var client = GetHttpClient(cookie, version);

            var paramsStr = JsonConvert.SerializeObject(parameters);
            HttpContent content = new StringContent(paramsStr, Encoding.UTF8, "application/json");
            content.Headers.ContentType = MediaTypeHeaderValue.Parse($"application/json{version}");

            try
            {
                var response = await client.PostAsync(url, content, cancellationToken);
                if (response.StatusCode == expectedStatusCode)
                {
                    return response;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(message);
                }
            }
            catch (OperationCanceledException e)
            {
                throw new OperationCanceledException(e.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private HttpClient GetHttpClient(string cookie, string version = null)
        {
            if (Client != null) return Client;
            Client = new HttpClient
            {
                //Timeout = TimeSpan.FromSeconds(TimeoutSeconds)
                Timeout = Timeout.InfiniteTimeSpan
            };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                MediaTypeWithQualityHeaderValue.Parse($"application/json{version}"));
            Client.DefaultRequestHeaders.Add("Cookie", cookie);
            //Client.DefaultRequestHeaders.Connection.Add("close");
            Client.DefaultRequestHeaders.Connection.Add("keep-alive");
            return Client;
        }
    }
}