using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private ReturnMerchandiseAuthorizationDto _returnMerchandiseAuthorization;

        [ObservableProperty]
        private ObservableCollection<RmaDetailLineViewModel> _lines;

        public RmaDetailViewModel(IApiService apiService, IServiceProvider serviceProvider, ReturnMerchandiseAuthorizationDto returnMerchandiseAuthorization)
        {
            _apiService = apiService;
            _serviceProvider = serviceProvider;
            _returnMerchandiseAuthorization = returnMerchandiseAuthorization;
            _lines = new ObservableCollection<RmaDetailLineViewModel>(
               returnMerchandiseAuthorization.Lines.Select(line => new RmaDetailLineViewModel(line))
            );
        }

        [RelayCommand]
        private async Task Scan()
        {
            var scanPage = _serviceProvider.GetRequiredService<ScanPage>();
            await Shell.Current.Navigation.PushAsync(scanPage);
        }
    }
}
