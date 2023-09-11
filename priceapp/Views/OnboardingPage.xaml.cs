using System;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class OnboardingPage
{
    private readonly IOnboardingViewModel _onboardingViewModel = DependencyService.Get<IOnboardingViewModel>(DependencyFetchTarget.NewInstance);
    private readonly IUserService _userService = DependencyService.Get<IUserService>(DependencyFetchTarget.NewInstance);

    public OnboardingPage()
    {
        InitializeComponent();

        BindingContext = _onboardingViewModel;
    }

    private async void SkipButton_OnClicked(object sender, EventArgs e)
    {
        var isLoggedIn = await _userService.IsUserLoggedIn();
        if (isLoggedIn)
        {
            Application.Current.MainPage = new MainPage();
        }
        else
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    private async void NextButton_OnClicked(object sender, EventArgs e)
    {
        if (_onboardingViewModel.Position == _onboardingViewModel.Items.Count - 1)
        {
            var isLoggedIn = await _userService.IsUserLoggedIn();
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