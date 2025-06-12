using Microsoft.Extensions.Logging;
using Warehouse.App.MVVM.Services;
using Warehouse.App.MVVM.ViewModels;
using Warehouse.App.MVVM.Views;
using ZXing.Net.Maui.Controls;

namespace Warehouse.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient(AppConstants.AuthHttpClientName, httpClient => httpClient.BaseAddress = new Uri("http://172.25.80.1:8081"));
        builder.Services.AddHttpClient(AppConstants.ApiClientName, httpClient =>
            httpClient.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5014" : "http://localhost:5014"));

        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IApiService, ApiService>();

        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<BookingViewModel>();
        builder.Services.AddTransient<RmaDetailViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<BookingPage>();
        builder.Services.AddTransient<ScanPage>();
        builder.Services.AddTransient<RmaDetailPage>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

