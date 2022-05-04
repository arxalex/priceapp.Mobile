using priceapp.Utils;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;
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
            
            var loginViewModel = DependencyService.Get<ILoginViewModel>();

            var isLoggedIn = loginViewModel.IsUserLoggedIn();
            if (isLoggedIn)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new LoginPage();
            }

            var geolocationUtil = DependencyService.Get<GeolocationUtil>();
            geolocationUtil?.GetCurrentLocationNow();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}