using System.Net.Http.Headers;
using System.Text.Json;
using Warehouse.App.MVVM.Models;
using Warehouse.Shared.Models;

namespace Warehouse.App.MVVM.Services
{
    public interface IApiService
    {
        Task<ApiResult<List<ReturnMerchandiseAuthorizationDto>>> GetAllRmasAsync();
        Task<ApiResult<ReturnMerchandiseAuthorizationDto>> GetRmaByTrackAndTraceAsync(string code);
    }
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IAuthService _authService;

        public ApiService(IHttpClientFactory httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;

        }

        public Task<ApiResult<List<ReturnMerchandiseAuthorizationDto>>> GetAllRmasAsync()
            => GetAsync<List<ReturnMerchandiseAuthorizationDto>>("api/app/returnMerchandiseAuthorizations");

        public Task<ApiResult<ReturnMerchandiseAuthorizationDto>> GetRmaByTrackAndTraceAsync(string code)
            => GetAsync<ReturnMerchandiseAuthorizationDto>($"api/app/returnMerchandiseAuthorization/byTrackAndTrace/{code}");

        private async Task<HttpClient> CreateAuthorizedHttpClientAsync()
        {
            var httpClient = _httpClient.CreateClient(AppConstants.ApiClientName);
            var token = await _authService.GetAccessTokenAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return httpClient;
        }

        private async Task<ApiResult<T>> GetAsync<T>(string url)
        {
            var httpClient = await CreateAuthorizedHttpClientAsync();
            var response = await httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ApiResult<T> { Data = data };
            }
            else
            {
                return new ApiResult<T> { ErrorMessage = content };
            }
        }

    }
}
