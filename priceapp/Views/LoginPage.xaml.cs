using System;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {
        private readonly ILoginViewModel _loginViewModel;

        public LoginPage()
        {
            InitializeComponent();
            _loginViewModel = DependencyService.Get<ILoginViewModel>(DependencyFetchTarget.NewInstance);
            _loginViewModel.LoginSuccess += LoginViewModelOnLoginSuccess;
        }

        private void LoginViewModelOnLoginSuccess(object sender, ProcessedArgs args)
        {
            if (args.Success)
            {
                if (VersionTracking.IsFirstLaunchEver)
                {
                    Application.Current.MainPage = new OnboardingPage();
                }
                else
                {
                    Application.Current.MainPage = new MainPage();
                }
            }
            else
            {
                ProcessedFrame.IsVisible = true;
                ProcessedLabel.Text = args.Message;
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await _loginViewModel.LoginUser(EntryEmail.Text, EntryPassword.Text);
        }

        private async void ButtonLoginAsGuest_OnClicked(object sender, EventArgs e)
        {
            await _loginViewModel.LoginAsGuest();
        }

        private void ButtonRegistration_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();
        }
    }
}