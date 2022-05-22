using System;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
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
            _loginViewModel = DependencyService.Get<ILoginViewModel>();
            _loginViewModel.LoginSuccess += LoginViewModelOnLoginSuccess;
        }

        private void LoginViewModelOnLoginSuccess(object sender, ProcessedArgs args)
        {
            if (args.Success)
            {
                Application.Current.MainPage = new MainPage();
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

        private async void ButtonRegistration_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}