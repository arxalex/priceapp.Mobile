using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class OnboardingPage
{
    private readonly IOnboardingViewModel _onboardingViewModel;
    private readonly IUserService _userService;
    private readonly IServiceProvider _serviceProvider;

    public OnboardingPage(IOnboardingViewModel onboardingViewModel, IUserService userService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _onboardingViewModel = onboardingViewModel;
        _userService = userService;
        _serviceProvider = serviceProvider;
        BindingContext = _onboardingViewModel;
    }

    private async void SkipButton_OnClicked(object sender, EventArgs e)
    {
        var isLoggedIn = await _userService.IsUserLoggedIn();
        if (isLoggedIn)
        {
            Application.Current.Windows[0].Page = new MainPage();
        }
        else
        {
            Application.Current.Windows[0].Page = new NavigationPage(new LoginPage(_serviceProvider.GetRequiredService<ILoginViewModel>(), _serviceProvider));
        }
    }

    private async void NextButton_OnClicked(object sender, EventArgs e)
    {
        if (_onboardingViewModel.Position == _onboardingViewModel.Items.Count - 1)
        {
            var isLoggedIn = await _userService.IsUserLoggedIn();
            if (isLoggedIn)
            {
                Application.Current.Windows[0].Page = new MainPage();
            }
            else
            {
                Application.Current.Windows[0].Page = new NavigationPage(new LoginPage(_serviceProvider.GetRequiredService<ILoginViewModel>(), _serviceProvider));
            }
        }
        else
        {
            _onboardingViewModel.Position++;
        }
    }
}