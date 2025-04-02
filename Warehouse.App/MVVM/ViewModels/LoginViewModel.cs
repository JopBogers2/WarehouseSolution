using System.Runtime.CompilerServices;
using System.Windows.Input;
using Warehouse.App.MVVM.Models;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.Views;

namespace Warehouse.App.MVVM.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService authService;

        private string? _username;
        private string? _password;
        private string? _errorMessage;
        private bool _isBusy = false;

        public LoginViewModel(IAuthService authService)
        {
            this.authService = authService;
            LoginCommand = new Command(OnLogin, CanLogin);
        }

        public string? Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public ICommand LoginCommand { get; }

        private bool CanLogin()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private async void OnLogin()
        {
            IsBusy = true;
            var error = await authService.LoginAsync(new LoginRequestDto(Username!, Password!));
            IsBusy = false;
            if (string.IsNullOrWhiteSpace(error))
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                return;
            }

            ErrorMessage = error;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            ((Command)LoginCommand).ChangeCanExecute();
        }
    }
}
