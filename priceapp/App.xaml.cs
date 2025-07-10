using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp;

public partial class App
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnectionService _connectionService;
    private readonly IUserService _userService;

    public App(IServiceProvider serviceProvider, IConnectionService connectionService, IUserService userService)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _connectionService = connectionService;
        _userService = userService;
        _connectionService.BadConnectEvent += ConnectionServiceOnBadConnectEvent;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(OnCreateWindow());
    }

    public async Page OnCreateWindow()
    {
        var isConnected = await _connectionService.IsConnectedAsync();
        if (!isConnected)
        {
            var isUserWasLoggedIn = await _userService.IsUserWasLoggedIn();
            if (isUserWasLoggedIn)
            {
                return new MainPage();
            }

            return new ConnectionErrorPage(new ConnectionErrorArgs
                { Message = "Відсутнє зʼєднання з сервером", StatusCode = 404, Success = false });
        }

        var needUpdate = await _connectionService.IsAppNeedsUpdateAsync();
        switch (needUpdate)
        {
            case true:
                return new UpdateAppPage();
        }

        var isLoggedIn = await _userService.IsUserLoggedIn();
        if (!isLoggedIn)
        {
            var loginResult = await _userService.LoginAsGuest();
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