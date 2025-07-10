#if ANDROID
using System.Net;
#endif
#if IOS
using Foundation;
#endif
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using priceapp.LocalDatabase.Repositories.Implementation;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;

namespace priceapp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIconsRegular.ttf", "Material");
                fonts.AddFont("MaterialIconsOutlinedRegular.ttf", "MaterialOutlined");
                fonts.AddFont("MaterialIconsRoundRegular.ttf", "MaterialRound");
                fonts.AddFont("MaterialIconsSharpRegular.ttf", "MaterialSharp");
                fonts.AddFont("MaterialIconsTwoToneRegular.ttf", "MaterialTwoTone");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddHttpClient("PriceApi")
#if ANDROID
            .ConfigurePrimaryHttpMessageHandler(() =>
                new Xamarin.Android.Net.AndroidMessageHandler());
#elif IOS
            .ConfigurePrimaryHttpMessageHandler(() =>
                new NSUrlSessionHandler()
            );
#endif

        
        builder.Services.AddSingleton(MapperUtil.CreateMapper());
        
        builder.Services.AddSingleton<IBrandAlertsLocalRepository, BrandAlertsLocalRepository>();
        builder.Services.AddSingleton<ICacheRequestsLocalRepository, CacheRequestsLocalRepository>();
        builder.Services.AddSingleton<ICategoriesLocalRepository, CategoriesLocalRepository>();
        builder.Services.AddSingleton<IFilialsLocalRepository, FilialsLocalRepository>();
        builder.Services.AddSingleton<IItemsLocalRepository, ItemsLocalRepository>();
        builder.Services.AddSingleton<IItemsToBuyLocalRepository, ItemsToBuyLocalRepository>();
        builder.Services.AddSingleton<IShopsLocalRepository, ShopsLocalRepository>();
        
        builder.Services.AddSingleton<IBrandAlertRepository, BrandAlertRepository>();
        builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
        builder.Services.AddSingleton<IInfoRepository, InfoRepository>();
        builder.Services.AddSingleton<IItemRepository, ItemRepository>();
        builder.Services.AddSingleton<IShopRepository, ShopRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        
        builder.Services
            .AddHttpClient<IBrandAlertRepository, BrandAlertRepository>(client =>
            {
                client.BaseAddress = new Uri(Constants.ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });
        builder.Services
            .AddHttpClient<ICategoryRepository, CategoryRepository>(client =>
            {
                client.BaseAddress = new Uri(Constants.ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });
        builder.Services
            .AddHttpClient<IInfoRepository, InfoRepository>(client =>
            {
                client.BaseAddress = new Uri(Constants.ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });
        builder.Services
            .AddHttpClient<IItemRepository, ItemRepository>(client =>
            {
                client.BaseAddress = new Uri(Constants.ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });
        builder.Services
            .AddHttpClient<IShopRepository, ShopRepository>(client =>
            {
                client.BaseAddress = new Uri(Constants.ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });
        builder.Services
            .AddHttpClient<IUserRepository, UserRepository>(client =>
            {
                client.BaseAddress = new Uri(Constants.ApiUrl);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

        
        builder.Services.AddSingleton<ICacheService, CacheService>();
        builder.Services.AddTransient<IConnectionService, ConnectionService>();
        builder.Services.AddTransient<ILocationService, LocationService>();
        builder.Services.AddTransient<IUserService, UserService>();
        
        builder.Services.AddTransient<IAccountViewModel, AccountViewModel>();
        builder.Services.AddTransient<ICartViewModel, CartViewModel>();
        builder.Services.AddTransient<ICategoryViewModel, CategoryViewModel>();
        builder.Services.AddTransient<IDeleteAccountViewModel, DeleteAccountViewModel>();
        builder.Services.AddTransient<IItemViewModel, ItemViewModel>();
        builder.Services.AddTransient<IItemsListViewModel, ItemsListViewModel>();
        builder.Services.AddTransient<ILoginViewModel, LoginViewModel>();
        builder.Services.AddTransient<IOnboardingViewModel, OnboardingViewModel>();
        builder.Services.AddTransient<IRegistrationViewModel, RegistrationViewModel>();
        builder.Services.AddTransient<ISearchItemsListViewModel, SearchItemsListViewModel>();
        builder.Services.AddTransient<ISearchViewModel, SearchViewModel>();
        builder.Services.AddTransient<ISettingsViewModel, SettingsViewModel>();

        return builder.Build();
    }
}