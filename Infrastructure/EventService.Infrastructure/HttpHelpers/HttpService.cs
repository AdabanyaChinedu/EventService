using EventService.Domain.Interfaces;
using System.Net.Http.Json;

namespace EventService.Infrastructure.HttpHelpers
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, uri);

                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"Error getting JSON from {uri}: {response.StatusCode}");
                    }

                    return await response.Content.ReadFromJsonAsync<T>()!;
                }
               
            }
            catch
            {
                throw;
            }
        }

    }
}
