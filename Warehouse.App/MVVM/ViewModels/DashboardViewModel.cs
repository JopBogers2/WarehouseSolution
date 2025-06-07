using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Warehouse.App.MVVM.Services;
using Warehouse.Shared.Models;

namespace Warehouse.App.MVVM.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly IApiService apiService;

        [ObservableProperty]
        private ObservableCollection<ReturnMerchandiseAuthorizationDto> _rmas = new();

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string? _errorMessage = null;

        [ObservableProperty]
        private string? _orderId;

        [ObservableProperty]
        private string? _distributionCenter;

        [ObservableProperty]
        private string? _platform;

        [ObservableProperty]
        private string? _channel;

        [ObservableProperty]
        private int _rmaCount;

        public DashboardViewModel(IApiService apiService)
        {
            this.apiService = apiService;
            LoadRmasCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadRmasAsync()
        {
            await SearchRmasAsync();
        }

        [RelayCommand]
        private async Task SearchRmasAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                var result = await apiService.SearchRmasAsync(OrderId, DistributionCenter, Platform, Channel);
                if (result.IsSuccess && result.Data != null)
                {
                    Rmas = new ObservableCollection<ReturnMerchandiseAuthorizationDto>(result.Data);
                    RmaCount = Rmas.Count;
                }
                else
                {
                    ErrorMessage = result.ErrorMessage ?? "Failed to load RMAs.";
                    RmaCount = 0;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
