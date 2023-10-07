using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.Utils;
using priceapp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

[assembly: ExportFont("MaterialIconsRegular.ttf", Alias = "Material")]
[assembly: ExportFont("MaterialIconsOutlinedRegular.otf", Alias = "MaterialOutlined")]
[assembly: ExportFont("MaterialIconsRoundRegular.otf", Alias = "MaterialRound")]
[assembly: ExportFont("MaterialIconsSharpRegular.otf", Alias = "MaterialSharp")]
[assembly: ExportFont("MaterialIconsTwoToneRegular.otf", Alias = "MaterialTwoTone")]

namespace priceapp
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            DependencyService.RegisterSingleton(MapperUtil.CreateMapper());
            MainPage = new Page();
        }

        protected override async void OnStart()
        {
            var connectionService = DependencyService.Get<IConnectionService>(DependencyFetchTarget.NewInstance);
            var userService = DependencyService.Get<IUserService>(DependencyFetchTarget.NewInstance);
            var locationService = DependencyService.Get<ILocationService>(DependencyFetchTarget.NewInstance);
            if (VersionTracking.IsFirstLaunchEver)
            {
                await locationService.RefreshPermission();
            }

            await locationService.RefreshLocation();

            connectionService.BadConnectEvent += ConnectionServiceOnBadConnectEvent;

            var isConnected = await connectionService.IsConnectedAsync();
            if (!isConnected)
            {
                var isUserWasLoggedIn = await userService.IsUserWasLoggedIn();
                if (isUserWasLoggedIn)
                {
                    MainPage = new MainPage();
                    return;
                }
                
                MainPage = new ConnectionErrorPage(new ConnectionErrorArgs(){Message = "Відсутнє зʼєднання з сервером", StatusCode = 404, Success = false});
                return;
            }

            var needUpdate = await connectionService.IsAppNeedsUpdateAsync();
            switch (needUpdate)
            {
                case null:
                    return;
                case true:
                    MainPage = new UpdateAppPage();
                    return;
            }

            var isLoggedIn = await userService.IsUserLoggedIn();
            if (!isLoggedIn)
            {
                var loginResult = await userService.LoginAsGuest();
                if (!loginResult.Success)
                {
                    MainPage = new ConnectionErrorPage(new ConnectionErrorArgs()
                    {
                        Message = loginResult.Message,
                        StatusCode = 400,
                        Success = loginResult.Success
                    });
                }
            }
            
            if (VersionTracking.IsFirstLaunchEver)
            {
                MainPage = new OnboardingPage();
                return;
            }

            MainPage = new MainPage();
        }

        private void ConnectionServiceOnBadConnectEvent(object sender, ConnectionErrorArgs args)
        {
            MainPage = new ConnectionErrorPage(args);
        }
    }
}