using Warehouse.App.MVVM.Services;

namespace Warehouse.App.MVVM.Views;

public partial class HomePage : ContentPage
{
    private readonly IAuthService AuthService;

    public HomePage(IAuthService authService)
    {
        InitializeComponent();
        AuthService = authService;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        AuthService.Logout();
    }
}