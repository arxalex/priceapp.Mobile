using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class LoginPage
{
    private readonly ILoginViewModel _loginViewModel;
    private readonly IServiceProvider _serviceProvider;

    public LoginPage(ILoginViewModel loginViewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _loginViewModel = loginViewModel;
        _serviceProvider = serviceProvider;

        _loginViewModel.LoginSuccess += LoginViewModelOnLoginSuccess;
    }

    private void LoginViewModelOnLoginSuccess(object sender, ProcessedArgs args)
    {
        if (args.Success)
        {
            if (VersionTracking.IsFirstLaunchEver)
            {
                Application.Current.Windows[0].Page = new OnboardingPage(_serviceProvider.GetRequiredService<IOnboardingViewModel>(), _serviceProvider.GetRequiredService<IUserService>(), _serviceProvider);
            }
            else
            {
                Application.Current.Windows[0].Page = new MainPage();
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
        Application.Current.Windows[0].Page = new RegisterPage(_serviceProvider.GetRequiredService<IRegistrationViewModel>(), _serviceProvider);
    }
}