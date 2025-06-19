using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Warehouse.App.MVVM.Models;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.Views;

namespace Warehouse.App.MVVM.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _valid = false;

        [ObservableProperty]
        private bool _loading = false;

        public bool CanLogin => Valid && !Loading;

        public LoginViewModel(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
        }

        partial void OnUsernameChanged(string value)
        {
            Validate();
        }
        partial void OnValidChanged(bool value)
        {
            OnPropertyChanged(nameof(CanLogin));
        }

        partial void OnLoadingChanged(bool value)
        {
            OnPropertyChanged(nameof(CanLogin));
        }

        partial void OnPasswordChanged(string value)
        {
            Validate();
        }

        private void Validate()
        {
            Valid = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        [RelayCommand]
        private async Task Login()
        {
            Loading = true;
            var error = await _authService.LoginAsync(new LoginRequestDto(Username, Password));
            Loading = false;
            if (string.IsNullOrWhiteSpace(error))
            {
                await _navigationService.GoToAsync($"//{nameof(DashboardPage)}");
                return;
            }

            ErrorMessage = error;
        }
    }
}
