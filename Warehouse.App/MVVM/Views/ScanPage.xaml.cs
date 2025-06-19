namespace Warehouse.App.MVVM.Views;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.ViewModels;
using ZXing.Net.Maui;

public partial class ScanPage : ContentPage
{
    private readonly INavigationService _navigationService;
    private readonly IApiService _apiService;
    public readonly IPageService _pageService;

    private readonly HashSet<string> _searchedBarcodes = new();

    public ScanPage(INavigationService navigationService, IApiService apiService, IPageService pageService)
    {
        InitializeComponent();
        _navigationService = navigationService;
        _apiService = apiService;
        _pageService = pageService;
        BarcodeReader.Options = new BarcodeReaderOptions
        {
            AutoRotate = true,
            Multiple = true
        };
    }


    protected async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        foreach (var barcode in e.Results)
        {
            if (_searchedBarcodes.Contains(barcode.Value))
                continue;

            _searchedBarcodes.Add(barcode.Value);

            var result = await _apiService.GetRmaByTrackAndTraceAsync(barcode.Value);
            if (result.IsSuccess && result.Data != null)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    BarcodeReader.IsDetecting = false;
                    BarcodeReader.Handler?.DisconnectHandler();

                    var viewModel = new RmaDetailViewModel(_apiService, _navigationService, result.Data);

                    await _navigationService.PushAsync(_pageService.CreateRmaDetailPage(viewModel));
                    _navigationService.RemovePage(this);
                });

                break;
            }
            else
            {
                Console.WriteLine($"Error: {result.ErrorMessage ?? "Unknown error"} for barcode {barcode.Value}");
            }
        }
    }
}