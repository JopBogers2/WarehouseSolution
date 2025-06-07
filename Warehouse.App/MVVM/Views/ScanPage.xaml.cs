namespace Warehouse.App.MVVM.Views;

using Warehouse.App.MVVM.Services;
using ZXing.Net.Maui;

public partial class ScanPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IApiService _apiService;

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
            var test = Navigation.ModalStack.Count;

            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}