using Warehouse.App.MVVM.ViewModels;

namespace Warehouse.App.MVVM.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}