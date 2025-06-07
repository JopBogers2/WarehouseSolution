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
        private string? _trackAndTrace;

        [ObservableProperty]
        private int _rmaCount;

        [ObservableProperty]
        private int _pageNumber = 1;

        [ObservableProperty]
        private int _pageSize = 20;

        [ObservableProperty]
        private int _totalCount;

        [ObservableProperty]
        private int _totalPages;

        public DashboardViewModel(IApiService apiService)
        {
            this.apiService = apiService;
            LoadRmasCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadRmasAsync()
        {
            PageNumber = 1;
            await GetRmasAsync();
        }


        [RelayCommand]
        private async Task GetRmasAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;
                var result = await apiService.SearchRmasAsync(OrderId, DistributionCenter, Platform, Channel, TrackAndTrace, PageNumber, PageSize);
                if (result.IsSuccess && result.Data != null)
                {
                    Rmas = new ObservableCollection<ReturnMerchandiseAuthorizationDto>(result.Data.Items);
                    RmaCount = result.Data.Items.Count;
                    TotalCount = result.Data.TotalCount;
                    TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
                }
                else
                {
                    ErrorMessage = result.ErrorMessage ?? "Failed to load RMAs.";
                    RmaCount = 0;
                    TotalCount = 0;
                    TotalPages = 1;
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task NextPageAsync()
        {
            if (PageNumber < TotalPages)
            {
                PageNumber++;
                await GetRmasAsync();
            }
        }

        [RelayCommand]
        private async Task PreviousPageAsync()
        {
            if (PageNumber > 1)
            {
                PageNumber--;
                await GetRmasAsync();
            }
        }
    }
}
