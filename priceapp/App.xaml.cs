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
            var geolocationUtil = DependencyService.Get<GeolocationUtil>();
            await geolocationUtil.GetCurrentLocationNow();

            var isConnected = await connectionService.IsConnectedAsync();
            if (!isConnected)
            {
                var isUserWasLoggedIn = await userService.IsUserWasLoggedIn();
                if (isUserWasLoggedIn)
                {
                    MainPage = new MainPage();
                    return;
                }
                
                MainPage = new NavigationPage(new LoginPage());
                return;
            }
            
            if (await connectionService.IsAppNeedsUpdateAsync())
            {
                MainPage = new UpdateAppPage();
                return;
            }
            
            var isLoggedIn = await userService.IsUserLoggedIn();
            if (isLoggedIn)
            {
                if (VersionTracking.IsFirstLaunchEver)
                {
                    MainPage = new OnboardingPage();
                    return;
                }

                MainPage = new MainPage();
                return;
            }
            
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}