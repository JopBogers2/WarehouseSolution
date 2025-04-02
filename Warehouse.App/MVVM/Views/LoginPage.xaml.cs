using Warehouse.App.MVVM.ViewModels;

namespace Warehouse.App.MVVM.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}