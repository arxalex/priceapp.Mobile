using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class RegisterPage
{
    private readonly IRegistrationViewModel _registrationViewModel;
    private readonly IServiceProvider _serviceProvider;

    public RegisterPage(IRegistrationViewModel registrationViewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _registrationViewModel = registrationViewModel;
        _serviceProvider = serviceProvider;
        _registrationViewModel.RegisterSuccess += RegistrationViewModelOnRegisterSuccess;
    }

    private void RegistrationViewModelOnRegisterSuccess(object sender, ProcessedArgs args)
    {
        if (args.Success)
        {
            Application.Current.Windows[0].Page = new NavigationPage(new ConfirmEmailPage(_serviceProvider));
        }
        else
        {
            ProcessedFrame.IsVisible = true;
            ProcessedLabel.Text = args.Message;
        }
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        await _registrationViewModel.RegisterUser(EntryUsername.Text, EntryEmail.Text, EntryPassword.Text);
    }

    private void ButtonLogin_OnClicked(object sender, EventArgs e)
    {
        Application.Current.Windows[0].Page = new LoginPage(_serviceProvider.GetRequiredService<ILoginViewModel>(), _serviceProvider);
    }
}