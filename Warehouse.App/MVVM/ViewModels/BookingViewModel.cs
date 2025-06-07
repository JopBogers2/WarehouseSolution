using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.Views;

namespace Warehouse.App.MVVM.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        private readonly IServiceProvider _serviceProvider;


        public BookingViewModel(IApiService apiService, IServiceProvider serviceProvider)
        {
            _apiService = apiService;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task Scan()
        {
            var scanPage = _serviceProvider.GetRequiredService<ScanPage>();
            await Shell.Current.Navigation.PushAsync(scanPage);
        }
    }
}
