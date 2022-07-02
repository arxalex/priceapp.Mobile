using System;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class OnboardingPage : ContentPage
{
    private readonly ILoginViewModel _loginViewModel;
    private readonly IOnboardingViewModel _onboardingViewModel;

    public OnboardingPage()
    {
        InitializeComponent();

        _onboardingViewModel = DependencyService.Get<IOnboardingViewModel>(DependencyFetchTarget.NewInstance);
        _loginViewModel = DependencyService.Get<ILoginViewModel>(DependencyFetchTarget.NewInstance);

        BindingContext = _onboardingViewModel;
    }

    private void SkipButton_OnClicked(object sender, EventArgs e)
    {
        var isLoggedIn = _loginViewModel.IsUserLoggedIn();
        if (isLoggedIn)
        {
            Application.Current.MainPage = new MainPage();
        }
        else
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    private void NextButton_OnClicked(object sender, EventArgs e)
    {
        if (_onboardingViewModel.Position == _onboardingViewModel.Items.Count - 1)
        {
            var isLoggedIn = _loginViewModel.IsUserLoggedIn();
            if (isLoggedIn)
            {
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        else
        {
            _onboardingViewModel.Position++;
        }
    }
}