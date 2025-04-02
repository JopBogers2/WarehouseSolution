using System.Net.Http.Headers;
using System.Text;
using Warehouse.App.MVVM.Models;
using Warehouse.App.MVVM.Views;

namespace Warehouse.App.MVVM.Services
{
    public interface IAuthService
    {
        Task<bool> IsUserAuthenticated();
        Task<string?> LoginAsync(LoginRequestDto dto);
        void Logout();
    }
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory HttpClient;

        public AuthService(IHttpClientFactory httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var token = await SecureStorage.Default.GetAsync(AppConstants.JwtTokenKeyName);

            if (!string.IsNullOrWhiteSpace(token))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string?> LoginAsync(LoginRequestDto dto)
        {
            var httpClient = HttpClient.CreateClient(AppConstants.AuthHttpClientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{dto.Username}:{dto.Password}")));

            var response = await httpClient.GetAsync("/api/v1/authenticate");

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                await SecureStorage.Default.SetAsync(AppConstants.JwtTokenKeyName, content.Result);
                return null;
            }
            else
            {
                return "Invalid credentials";
            }
        }

        public async void Logout()
        {
            SecureStorage.Default.Remove(AppConstants.JwtTokenKeyName);
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

    }
}
