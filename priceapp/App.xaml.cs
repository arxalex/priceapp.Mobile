using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp;

public partial class App
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage());
    }

    public Page OnCreateWindow()
    {
        var connectionService = _serviceProvider.GetRequiredService<IConnectionService>();
        var userService = _serviceProvider.GetRequiredService<IUserService>();
        var locationService = _serviceProvider.GetRequiredService<ILocationService>();
        
        if (VersionTracking.IsFirstLaunchEver)
        {
            locationService.RefreshPermission().Wait();
        }

        locationService.RefreshLocation().Wait();

        connectionService.BadConnectEvent += ConnectionServiceOnBadConnectEvent;

        var isConnected = connectionService.IsConnectedAsync().Result;
        if (!isConnected)
        {
            var isUserWasLoggedIn = userService.IsUserWasLoggedIn().Result;
            if (isUserWasLoggedIn)
            {
                return new MainPage();
            }

            return new ConnectionErrorPage(new ConnectionErrorArgs
                { Message = "Відсутнє зʼєднання з сервером", StatusCode = 404, Success = false });
        }

        var needUpdate = connectionService.IsAppNeedsUpdateAsync().Result;
        switch (needUpdate)
        {
            case true:
                return new UpdateAppPage();
        }

        var isLoggedIn = userService.IsUserLoggedIn().Result;
        if (!isLoggedIn)
        {
            var loginResult = userService.LoginAsGuest().Result;
            if (!loginResult.Success)
            {
                return new ConnectionErrorPage(new ConnectionErrorArgs
                {
                    Message = loginResult.Message,
                    StatusCode = 400,
                    Success = loginResult.Success
                });
            }
        }

        if (VersionTracking.IsFirstLaunchEver)
        {
            return new OnboardingPage(_serviceProvider.GetRequiredService<IOnboardingViewModel>(), _serviceProvider.GetRequiredService<IUserService>(), _serviceProvider);
        }

        return new MainPage();
    }

    private void ConnectionServiceOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Windows[0].Page = new ConnectionErrorPage(args);
    }
}