using Warehouse.App.MVVM.ViewModels;

namespace Warehouse.App.MVVM.Views;

public partial class RmaDetailPage : ContentPage
{
    public RmaDetailPage(RmaDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}