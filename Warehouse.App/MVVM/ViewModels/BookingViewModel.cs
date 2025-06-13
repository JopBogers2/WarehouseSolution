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

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string? _errorMessage = null;

        [ObservableProperty]
        private string? _trackAndTrace;
        public BookingViewModel(IApiService apiService, IServiceProvider serviceProvider)
        {
            _apiService = apiService;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task ScanAsync()
        {
            var scanPage = _serviceProvider.GetRequiredService<ScanPage>();
            await Shell.Current.Navigation.PushAsync(scanPage);
        }

        [RelayCommand]
        private async Task SearchRmaAsync()
        {
            if (TrackAndTrace != null)
            {
                try
                {
                    IsLoading = true;
                    ErrorMessage = null;
                    var result = await _apiService.GetRmaByTrackAndTraceAsync(TrackAndTrace);

                    if (result.IsSuccess && result.Data != null)
                    {
                        var viewModel = new RmaDetailViewModel(_apiService, _serviceProvider, result.Data);
                        var rmaDetailPage = new RmaDetailPage(viewModel);

                        await Shell.Current.Navigation.PushAsync(rmaDetailPage);
                    }
                    else
                    {
                        ErrorMessage = result.ErrorMessage ?? "Failed to load RMAs.";
                    }
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }
    }
}
