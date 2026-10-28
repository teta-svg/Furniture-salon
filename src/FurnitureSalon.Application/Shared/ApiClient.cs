using System.Net.Http;
using System.Net.Http.Json;

namespace FurnitureSalon.Application.Shared
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            using var response = await _httpClient.PostAsJsonAsync(endpoint, request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}