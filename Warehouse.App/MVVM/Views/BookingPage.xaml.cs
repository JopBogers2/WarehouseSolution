using Warehouse.App.MVVM.ViewModels;

namespace Warehouse.App.MVVM.Views;

public partial class BookingPage : ContentPage
{
    public BookingPage(BookingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}