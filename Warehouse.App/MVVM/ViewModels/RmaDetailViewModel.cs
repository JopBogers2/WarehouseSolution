using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Warehouse.App.MVVM.Services;
using Warehouse.Shared.Models;

namespace Warehouse.App.MVVM.ViewModels
{
    public partial class RmaDetailViewModel : ObservableObject
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ReturnMerchandiseAuthorizationDto _returnMerchandiseAuthorization;

        [ObservableProperty]
        private ObservableCollection<RmaDetailLineViewModel> _lines;

        [ObservableProperty]
        private string? _errorMessage;

        public RmaDetailViewModel(IApiService apiService, INavigationService navigationService, ReturnMerchandiseAuthorizationDto returnMerchandiseAuthorization)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            _returnMerchandiseAuthorization = returnMerchandiseAuthorization;
            _lines = new ObservableCollection<RmaDetailLineViewModel>(
               returnMerchandiseAuthorization.Lines.Select(line => new RmaDetailLineViewModel(line))
            );
        }

        [RelayCommand]
        private void DeleteLine(RmaDetailLineViewModel line)
        {
            if (line != null)
                Lines.Remove(line);
        }

        [RelayCommand]
        private async Task BookReturn()
        {
            if (Lines.Count == 0)
            {
                ErrorMessage = "At least one line is required to book an RMA.";
                return;
            }

            if (Lines.Any(line => line.Quantity <= 0))
            {
                ErrorMessage = "All lines must have a quantity greater than zero.";
                return;
            }

            if (Lines.Any(line => string.IsNullOrWhiteSpace(line.Condition) || string.IsNullOrWhiteSpace(line.Resolution)))
            {
                ErrorMessage = "All lines must have a Condition and Resolution specified.";
                return;
            }

            ErrorMessage = null;

            var Return = new ReturnDto
            {
                Platform = ReturnMerchandiseAuthorization.Platform,
                Channel = ReturnMerchandiseAuthorization.Channel,
                OrderId = ReturnMerchandiseAuthorization.OrderId,
                ReturnRequestId = ReturnMerchandiseAuthorization.ReturnRequestId,
                Lines = Lines.Select(line => new ReturnLineDto
                {
                    LineId = line.LineId,
                    ArticleCode = line.ArticleCode,
                    Quantity = line.Quantity,
                    DistributionCenter = ReturnMerchandiseAuthorization.DistributionCenter,
                    Reason = line.Reason ?? string.Empty,
                    Condition = line.Condition ?? string.Empty
                }).ToList()
            };

            var result = await _apiService.PostReturn(Return);


            if (result.IsSuccess && result.Data != null)
            {
                await _navigationService.PopAsync();
            }
            else
            {
                ErrorMessage = string.IsNullOrWhiteSpace(result.ErrorMessage) ? "Failed to book return" : result.ErrorMessage;
            }

        }
    }
}
