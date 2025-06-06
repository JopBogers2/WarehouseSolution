using Warehouse.App.MVVM.Services;

namespace Warehouse.App.MVVM.Views;

public partial class BookingPage : ContentPage
{
    public BookingPage(IApiService _apiService)
    {
        InitializeComponent();
    }
}