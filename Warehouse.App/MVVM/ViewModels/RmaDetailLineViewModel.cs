using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Warehouse.Shared.Models;

namespace Warehouse.App.MVVM.ViewModels
{
    public partial class RmaDetailLineViewModel : ObservableObject
    {
        private readonly ReturnMerchandiseAuthorizationLineDto _line;


        [ObservableProperty]
        private int _lineId;

        [ObservableProperty]
        private string _articleCode;

        [ObservableProperty]
        private int _quantity;

        [ObservableProperty]
        private string? _reason;

        [ObservableProperty]
        private string? _resolution;

        [ObservableProperty]
        private string? _condition;

        public RmaDetailLineViewModel(ReturnMerchandiseAuthorizationLineDto line)
        {
            _line = line;
            LineId = line.LineId;
            ArticleCode = line.ArticleCode;
            Quantity = line.Quantity;
            Reason = line.Reason;
            Resolution = line.Resolution;
        }

        [RelayCommand]
        private void IncrementQuantity()
        {
            Quantity++;
        }

        [RelayCommand]
        private void DecrementQuantity()
        {
            if (Quantity > 0)
                Quantity--;
        }
    }
}
