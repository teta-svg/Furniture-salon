using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
        {
            using var response = await _httpClient.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PutAsync<TRequest>(string endpoint, TRequest request)
        {
            using var response = await _httpClient.PutAsJsonAsync(endpoint, request);

            await EnsureSuccessAsync(response);
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            string body = await response.Content.ReadAsStringAsync();

            string message = $"Сервер вернул ошибку {(int)response.StatusCode}.";

            if (body.TrimStart().StartsWith('"'))
            {
                try
                {
                    message = JsonSerializer.Deserialize<string>(body) ?? message;
                }
                catch (JsonException)
                {
                }
            }

            throw new HttpRequestException(message, null, response.StatusCode);
        }
    }
}