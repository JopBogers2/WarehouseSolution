using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.Views;
using Warehouse.Shared.Models;

namespace Warehouse.App.MVVM.ViewModels
{
    public partial class RmaDetailViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ReturnMerchandiseAuthorizationDto _returnMerchandiseAuthorization;

        public RmaDetailViewModel(IApiService apiService, IServiceProvider serviceProvider, ReturnMerchandiseAuthorizationDto returnMerchandiseAuthorization)
        {
            _apiService = apiService;
            _serviceProvider = serviceProvider;
            _returnMerchandiseAuthorization = returnMerchandiseAuthorization;
        }

        [RelayCommand]
        private async Task Scan()
        {
            var scanPage = _serviceProvider.GetRequiredService<ScanPage>();
            await Shell.Current.Navigation.PushAsync(scanPage);
        }
    }
}
