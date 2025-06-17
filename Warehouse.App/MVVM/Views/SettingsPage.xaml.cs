using Warehouse.App.MVVM.Services;

namespace Warehouse.App.MVVM.Views;

public partial class SettingsPage : ContentPage
{
    private readonly IAuthService authService;

    public SettingsPage(IAuthService _authService)
    {
        InitializeComponent();
        authService = _authService;
    }

    private void LogoutButton_Clicked(object sender, EventArgs e)
    {
        authService.Logout();
    }
}