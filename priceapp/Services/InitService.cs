using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.Views;

namespace priceapp.Services;

public static class InitService
{
    public static async void OnStart(IServiceProvider serviceProvider)
    {
        Application.Current.Windows[0].Page = await GetPage(serviceProvider);
    }
    private static async Task<Page> GetPage(IServiceProvider serviceProvider)
    {
        var connectionService = serviceProvider.GetRequiredService<IConnectionService>();
        var userService = serviceProvider.GetRequiredService<IUserService>();
        connectionService.BadConnectEvent += ConnectionServiceOnBadConnectEvent;
        var isConnected = await connectionService.IsConnectedAsync();
        if (!isConnected)
        {
            var isUserWasLoggedIn = await userService.IsUserWasLoggedIn();
            if (isUserWasLoggedIn)
            {
                return new MainPage();
            }

            return new ConnectionErrorPage(new ConnectionErrorArgs
                { Message = "Відсутнє зʼєднання з сервером", StatusCode = 404, Success = false });
        }

        var needUpdate = await connectionService.IsAppNeedsUpdateAsync();
        switch (needUpdate)
        {
            case true:
                return new UpdateAppPage();
        }

        var isLoggedIn = await userService.IsUserLoggedIn();
        if (!isLoggedIn)
        {
            var loginResult = await userService.LoginAsGuest();
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
            return new OnboardingPage();
        }

        return new MainPage();
    }

    private static void ConnectionServiceOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Application.Current.Windows[0].Page = new ConnectionErrorPage(args);
    }
}