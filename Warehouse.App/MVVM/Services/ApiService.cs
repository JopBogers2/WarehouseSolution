using System.Net.Http.Headers;
using System.Text.Json;
using Warehouse.App.MVVM.Models;
using Warehouse.Shared.Models;

namespace Warehouse.App.MVVM.Services
{
    public interface IApiService
    {
        Task<ApiResult<List<ReturnMerchandiseAuthorizationDto>>> GetAllRmasAsync();
        Task<ApiResult<PagedResult<ReturnMerchandiseAuthorizationDto>>> SearchRmasAsync(
            string? orderId = null,
            string? distributionCenter = null,
            string? platform = null,
            string? channel = null,
            string? trackAndTrace = null,
            int pageNumber = 1,
            int pageSize = 20);
        Task<ApiResult<ReturnMerchandiseAuthorizationDto>> GetRmaByTrackAndTraceAsync(string code);
        Task<ApiResult<ReturnDto>> PostReturn(ReturnDto returnDto);
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

        public Task<ApiResult<PagedResult<ReturnMerchandiseAuthorizationDto>>> SearchRmasAsync(
            string? orderId = null,
            string? distributionCenter = null,
            string? platform = null,
            string? channel = null,
            string? trackAndTrace = null,
            int pageNumber = 1,
            int pageSize = 20)
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(orderId)) query.Add($"orderId={Uri.EscapeDataString(orderId)}");
            if (!string.IsNullOrWhiteSpace(distributionCenter)) query.Add($"distributionCenter={Uri.EscapeDataString(distributionCenter)}");
            if (!string.IsNullOrWhiteSpace(platform)) query.Add($"platform={Uri.EscapeDataString(platform)}");
            if (!string.IsNullOrWhiteSpace(channel)) query.Add($"channel={Uri.EscapeDataString(channel)}");
            if (!string.IsNullOrWhiteSpace(trackAndTrace)) query.Add($"trackAndTrace={Uri.EscapeDataString(trackAndTrace)}");
            query.Add($"pageNumber={pageNumber}");
            query.Add($"pageSize={pageSize}");
            var queryString = query.Count > 0 ? "?" + string.Join("&", query) : string.Empty;
            return GetAsync<PagedResult<ReturnMerchandiseAuthorizationDto>>($"api/app/returnMerchandiseAuthorizations/search{queryString}");
        }


        public Task<ApiResult<ReturnMerchandiseAuthorizationDto>> GetRmaByTrackAndTraceAsync(string code)
            => GetAsync<ReturnMerchandiseAuthorizationDto>($"api/app/returnMerchandiseAuthorization/byTrackAndTrace/{code}");

        public Task<ApiResult<ReturnDto>> PostReturn(ReturnDto returnDto)
            => PostAsync<ReturnDto>("api/app/book/return", returnDto);

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

        private async Task<ApiResult<T>> PostAsync<T>(string url, T data)
        {
            var httpClient = await CreateAuthorizedHttpClientAsync();
            var content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var resultData = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ApiResult<T> { Data = resultData };
            }
            else
            {
                return new ApiResult<T> { ErrorMessage = responseContent };
            }
        }

    }
}
