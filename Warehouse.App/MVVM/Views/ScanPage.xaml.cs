namespace Warehouse.App.MVVM.Views;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.ViewModels;
using ZXing.Net.Maui;

public partial class ScanPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IApiService _apiService;
    private readonly HashSet<string> _searchedBarcodes = new();

    public ScanPage(IServiceProvider serviceProvider, IApiService apiService)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _apiService = apiService;
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

                    var viewModel = new RmaDetailViewModel(_apiService, _serviceProvider, result.Data);
                    var rmaDetailPage = new RmaDetailPage(viewModel);

                    await Shell.Current.Navigation.PushAsync(rmaDetailPage);
                    Shell.Current.Navigation.RemovePage(this);
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