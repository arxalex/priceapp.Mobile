using System;
using priceapp.ViewModels.Interfaces;
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
    public partial class App : Application
    {
        private readonly ILoginViewModel _loginViewModel;
        public App()
        {
            InitializeComponent();
            
            _loginViewModel = DependencyService.Get<ILoginViewModel>();

            var isLoggedIn = _loginViewModel.IsUserLoggedIn();
            if (isLoggedIn == false)
            {
                Current.Properties["isLoggedIn"] = false;
                MainPage = new LoginPage();
            }
            else
            {
                MainPage = new MainPage();
            }
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