using System;
using System.Threading;
using System.Threading.Tasks;

namespace HttpOperation
{
    public interface ILocalClient
    {
        Task ExecuteRequestAsync<T>(T input, CancellationToken cancellationToken);
    }

    public class LocalClient : ILocalClient
    {
        private const string Route = "";
        private const string Cookie = "";
        
        private readonly IHttpService _httpService;
        private string _baseUrl;

        public string BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = value;
        }

        public LocalClient(string baseUrl, int timeOut = 60)
        {
            _httpService = new HttpService();
            _baseUrl = baseUrl;
        }

        private static string GetRequestUrl(string baseUrl, string relativeUrl)
        {
            return new Uri(new Uri(baseUrl + (baseUrl.EndsWith("/") ? "" : "/")), relativeUrl).ToString();
        }

        public async Task ExecuteRequestAsync<T>(T input, CancellationToken cancellationToken)
        {
            var url = GetRequestUrl(_baseUrl, Route);
            await _httpService.PostResponseAsync(url, input, Cookie, cancellationToken);
        }
    }
}