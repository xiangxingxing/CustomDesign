

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HttpOperation
{
    public interface ILocalClient
    {
        Task ExecuteRequestAsync(LyricRequestBody input, CancellationToken cancellationToken);
    }

    public class LocalClient : ILocalClient
    {
        private const string Route = "api/ProjItems/Cache";
        private const string Cookie =
            "ARRAffinity=690af806ed8fbb3086efb9e5a41fd5ab587d5e1f311572d2561a0f064c266964; ARRAffinitySameSite=690af806ed8fbb3086efb9e5a41fd5ab587d5e1f311572d2561a0f064c266964";
        
        private readonly IHttpService _httpService;
        private string _baseUrl;

        public string BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = value;
        }

        public LocalClient(string baseUrl, int timeOut = 60)
        {
            _httpService = new HttpService(timeOut);
            _baseUrl = baseUrl;
        }

        private static string GetRequestUrl(string baseUrl, string relativeUrl)
        {
            return new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/") ? "" : "/")), relativeUrl).ToString();
        }

        public async Task ExecuteRequestAsync(LyricRequestBody input, CancellationToken cancellationToken)
        {
            var url = GetRequestUrl(_baseUrl, Route);
            await _httpService.PostResponseAsync(url, input, Cookie, cancellationToken);
        }
    }
}